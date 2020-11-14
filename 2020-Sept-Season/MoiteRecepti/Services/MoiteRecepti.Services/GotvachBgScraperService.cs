namespace MoiteRecepti.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using AngleSharp;

    using MoiteRecepti.Data.Common.Repositories;
    using MoiteRecepti.Data.Models;
    using MoiteRecepti.Services.Models;

    public class GotvachBgScraperService : IGotvachBgScraperService
    {
        private readonly IConfiguration config;
        private readonly IBrowsingContext context;

        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<Ingredient> ingridientsRepository;
        private readonly IDeletableEntityRepository<Recipe> recipiesRepository;
        private readonly IRepository<RecipeIngredient> recipeIngredientsRepository;
        private readonly IRepository<Image> imagesRepository;

        public GotvachBgScraperService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<Ingredient> ingridientsRepository,
            IDeletableEntityRepository<Recipe> recipiesRepository,
            IRepository<RecipeIngredient> recipeIngredientsRepository,
            IRepository<Image> imagesRepository)
        {
            this.categoriesRepository = categoriesRepository;
            this.ingridientsRepository = ingridientsRepository;
            this.recipiesRepository = recipiesRepository;
            this.recipeIngredientsRepository = recipeIngredientsRepository;
            this.imagesRepository = imagesRepository;

            this.config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(this.config);
        }

        public async Task PopulateDbWithRecipesAsync(int recipesCount)
        {
            var concurentBag = new ConcurrentBag<RecipeDto>();

            Parallel.For(1, recipesCount, (i) =>
            {
                try
                {
                    var recipe = this.GetRecipe(i);
                    concurentBag.Add(recipe);
                }
                catch
                {
                }
            });

            foreach (var recipe in concurentBag)
            {
                var categoryId = await this.GetOrCreateCategoryAsync(recipe.CategoryName);

                var recipeExists = this.recipiesRepository
                    .AllAsNoTracking()
                    .Any(x => x.Name == recipe.RecipeName);

                if (recipeExists)
                {
                    continue;
                }

                var newRecipe = new Recipe()
                {
                    Name = recipe.RecipeName,
                    Instructions = recipe.Instrucitons,
                    PreparationTime = recipe.PreparationTime,
                    CookingTime = recipe.CookingTime,
                    PortionsCount = recipe.PortionsCount,
                    OriginalUrl = recipe.OriginalUrl,
                    CategoryId = categoryId,
                };

                await this.recipiesRepository.AddAsync(newRecipe);
                await this.recipiesRepository.SaveChangesAsync();

                foreach (var item in recipe.Ingredients)
                {
                    var ingr = item.Split(" - ", 2);

                    if (ingr.Length < 2)
                    {
                        continue;
                    }

                    var ingridientId = await this.GetOrCreateIngridientAsync(ingr[0].Trim());
                    var qty = ingr[1].Trim();

                    var recipeIngridient = new RecipeIngredient
                    {
                        IngredientId = ingridientId,
                        RecipeId = newRecipe.Id,
                        Quantity = qty,
                    };

                    await this.recipeIngredientsRepository.AddAsync(recipeIngridient);
                    await this.recipeIngredientsRepository.SaveChangesAsync();
                }

                var image = new Image
                {
                    Extension = recipe.OriginalUrl,
                    RecipeId = newRecipe.Id,
                };

                await this.imagesRepository.AddAsync(image);
                await this.imagesRepository.SaveChangesAsync();
            }
        }

        private async Task<int> GetOrCreateIngridientAsync(string name)
        {
            var ingridient = this.ingridientsRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Name == name);

            if (ingridient == null)
            {
                ingridient = new Ingredient
                {
                    Name = name,
                };

                await this.ingridientsRepository.AddAsync(ingridient);
                await this.ingridientsRepository.SaveChangesAsync();
            }

            return ingridient.Id;
        }

        private async Task<int> GetOrCreateCategoryAsync(string categoryName)
        {
            var catogory = this.categoriesRepository
                                .AllAsNoTracking()
                                .FirstOrDefault(x => x.Name == categoryName);

            if (catogory == null)
            {
                catogory = new Category()
                {
                    Name = categoryName,
                };

                await this.categoriesRepository.AddAsync(catogory);
                await this.categoriesRepository.SaveChangesAsync();
            }

            return catogory.Id;
        }

        private RecipeDto GetRecipe(int id)
        {
            var document = this.context
                .OpenAsync($"https://recepti.gotvach.bg/r-{id}")
                .GetAwaiter()
                .GetResult();

            if (document.StatusCode == HttpStatusCode.NotFound ||
                document.DocumentElement.OuterHtml.Contains("Тази страница не е намерена"))
            {
                throw new InvalidOperationException();
            }

            var recipe = new RecipeDto();

            var recipeNameAndCategory = document
                .QuerySelectorAll("#recEntity > div.breadcrumb")
                .Select(x => x.TextContent)
                .FirstOrDefault()
                .Split(" »", StringSplitOptions.RemoveEmptyEntries)
                .Reverse()
                .ToArray();

            recipe.CategoryName = recipeNameAndCategory[1];
            recipe.RecipeName = recipeNameAndCategory[0].TrimStart();

            var instructions = document.QuerySelectorAll(".text > p")
                .Select(x => x.TextContent)
                .ToList();

            var sb = new StringBuilder();
            foreach (var item in instructions)
            {
                sb.AppendLine(item);
            }

            recipe.Instrucitons = sb.ToString().TrimEnd();

            var timing = document.QuerySelectorAll(".mbox > .feat.small");

            if (timing.Length > 0)
            {
                var prepartionTime = timing[0]
                    .TextContent
                    .Replace("Приготвяне", string.Empty)
                    .Replace(" мин.", string.Empty);

                var totalMinutes = int.Parse(prepartionTime);

                recipe.PreparationTime = TimeSpan.FromMinutes(totalMinutes);
            }

            if (timing.Length > 1)
            {
                var cookingTime = timing[1]
                    .TextContent
                    .Replace("Готвене", string.Empty)
                    .Replace(" мин.", string.Empty);

                var totalMinutes = int.Parse(cookingTime);

                recipe.CookingTime = TimeSpan.FromMinutes(totalMinutes);
            }

            var portionsCount = document
                .QuerySelectorAll(".mbox > .feat > span")
                .LastOrDefault()
                .TextContent;

            recipe.PortionsCount = int.Parse(portionsCount);

            recipe.OriginalUrl = document.QuerySelector("#newsGal > div.image > img").GetAttribute("src");

            var ingridients = document.QuerySelectorAll(".products > ul > li");

            foreach (var item in ingridients)
            {
                recipe.Ingredients.Add(item.TextContent);
            }

            return recipe;
        }
    }
}
