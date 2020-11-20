namespace MoiteRecepti.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using AngleSharp;

    using MoiteRecepti.Data.Common.Repositories;
    using MoiteRecepti.Data.Models;
    using MoiteRecepti.Services.Models;

    public class GotvachBgScraperService : IGotvachBgScraperService
    {
        private const string BaseUrl = "https://recepti.gotvach.bg/r-{0}";

        private readonly IBrowsingContext context;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Ingredient> ingredientsRepository;
        private readonly IDeletableEntityRepository<Recipe> recipesRepository;
        private readonly IRepository<RecipeIngredient> recipeIngredientsRepository;
        private readonly IRepository<Image> imagesRepository;

        public GotvachBgScraperService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Ingredient> ingredientsRepository,
            IDeletableEntityRepository<Recipe> recipesRepository,
            IRepository<RecipeIngredient> recipeIngredientsRepository,
            IRepository<Image> imagesRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.ingredientsRepository = ingredientsRepository;
            this.recipesRepository = recipesRepository;
            this.recipeIngredientsRepository = recipeIngredientsRepository;
            this.imagesRepository = imagesRepository;

            var config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(config);
        }

        public async Task ImportRecipesAsync(int fromId = 1, int toId = 10000)
        {
            var concurrentBag = this.ScrapeRecipes(fromId, toId);
            Console.WriteLine($"Scraped recipes: {concurrentBag.Count}");

            int addedCount = 0;
            foreach (var recipe in concurrentBag)
            {
                var categoryId = await this.GetOrCreateCategoryAsync(recipe.CategoryName);

                if (recipe.CookingTime.Days >= 1)
                {
                    recipe.CookingTime = new TimeSpan(23, 59, 59);
                }

                if (recipe.PreparationTime.Days >= 1)
                {
                    recipe.PreparationTime = new TimeSpan(23, 59, 59);
                }

                var newRecipe = new Recipe
                {
                    Name = recipe.RecipeName,
                    Instructions = recipe.Instructions,
                    PreparationTime = recipe.PreparationTime,
                    CookingTime = recipe.CookingTime,
                    PortionsCount = recipe.PortionsCount,
                    OriginalUrl = recipe.OriginalUrl,
                    CategoryId = categoryId,
                };
                await this.recipesRepository.AddAsync(newRecipe);

                var ingredients = recipe.Ingredients
                    .Select(i => i.Split(" - ", 2))
                    .Where(i => i.Length >= 2)
                    .ToArray();

                foreach (var ingredient in ingredients)
                {
                    var ingredientId = await this.GetOrCreateIngredientAsync(ingredient[0].Trim());
                    var quantity = ingredient[1].Trim();

                    var recipeIngredient = new RecipeIngredient
                    {
                        IngredientId = ingredientId,
                        Recipe = newRecipe,
                        Quantity = quantity,
                    };
                    await this.recipeIngredientsRepository.AddAsync(recipeIngredient);
                }

                var image = new Image
                {
                    RemoteImageUrl = recipe.ImageUrl,
                    Recipe = newRecipe,
                };
                await this.imagesRepository.AddAsync(image);

                if (++addedCount % 1000 == 0)
                {
                    await this.recipesRepository.SaveChangesAsync();
                    Console.WriteLine($"Saved count: {addedCount}");
                }
            }

            await this.recipesRepository.SaveChangesAsync();
            Console.WriteLine($"Count: {addedCount}");
        }

        private ConcurrentBag<RecipeDto> ScrapeRecipes(int fromId, int toId)
        {
            var concurrentBag = new ConcurrentBag<RecipeDto>();
            Parallel.For(fromId, toId + 1, i =>
            {
                try
                {
                    var recipe = this.GetRecipe(i);
                    concurrentBag.Add(recipe);
                }
                catch
                {
                    // ignored
                }
            });
            return concurrentBag;
        }

        private RecipeDto GetRecipe(int id)
        {
            var url = string.Format(BaseUrl, id);

            var document = this.context
                .OpenAsync(url)
                .GetAwaiter()
                .GetResult();

            if (document.StatusCode == HttpStatusCode.NotFound ||
                document.DocumentElement.OuterHtml.Contains("Тази страница не е намерена"))
            {
                throw new InvalidOperationException();
            }

            var recipe = new RecipeDto();

            var recipeNameCategory = document
                .QuerySelectorAll("#recEntity > div.breadcrumb")
                .Select(x => x.TextContent)
                .FirstOrDefault()
                .Split(" »", StringSplitOptions.RemoveEmptyEntries)
                .Reverse()
                .ToArray();

            // Get category name
            recipe.CategoryName = recipeNameCategory[1].Trim();

            // Get recipe name
            recipe.RecipeName = recipeNameCategory[0].Trim();

            // Get instructions
            var instructions = document.QuerySelectorAll(".text > p")
                .Select(x => x.TextContent)
                .ToList();

            recipe.Instructions = string.Join(Environment.NewLine, instructions);

            var timing = document
                .QuerySelectorAll(".mbox > .feat.small");

            // Get preparation time
            if (timing.Length > 0)
            {
                var preparationTime = timing[0]
                    .TextContent
                    .Replace("Приготвяне", string.Empty)
                    .Replace(" мин.", string.Empty);

                var totalMinutes = int.Parse(preparationTime);

                recipe.PreparationTime = TimeSpan.FromMinutes(totalMinutes);
            }

            // Get cooking time
            if (timing.Length > 1)
            {
                var cookingTime = timing[1]
                    .TextContent
                    .Replace("Готвене", string.Empty)
                    .Replace(" мин.", string.Empty);

                var totalMinutes = int.Parse(cookingTime);

                recipe.CookingTime = TimeSpan.FromMinutes(totalMinutes);
            }

            // Get portions count
            var portionsCount = document
                .QuerySelectorAll(".mbox > .feat > span")
                .LastOrDefault()
                ?.TextContent;

            recipe.PortionsCount = int.Parse(portionsCount);

            // Get image url
            recipe.ImageUrl = document
                .QuerySelector("#newsGal > div.image > img")
                .GetAttribute("src");

            // Get ingredients
            var ingredients = document
                .QuerySelectorAll(".products > ul > li")
                .Select(x => x.TextContent)
                .ToList();

            recipe.Ingredients.AddRange(ingredients);

            // Get image url
            recipe.OriginalUrl = url;

            Console.WriteLine(id);
            return recipe;
        }

        private async Task<int> GetOrCreateIngredientAsync(string name)
        {
            var ingredient = this.ingredientsRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == name);

            if (ingredient != null)
            {
                return ingredient.Id;
            }

            ingredient = new Ingredient
            {
                Name = name,
            };

            await this.ingredientsRepository.AddAsync(ingredient);
            await this.ingredientsRepository.SaveChangesAsync();

            return ingredient.Id;
        }

        private async Task<int> GetOrCreateCategoryAsync(string categoryName)
        {
            var category = this.categoriesRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == categoryName);

            if (category != null)
            {
                return category.Id;
            }

            category = new Category
            {
                Name = categoryName,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            return category.Id;
        }
    }
}
