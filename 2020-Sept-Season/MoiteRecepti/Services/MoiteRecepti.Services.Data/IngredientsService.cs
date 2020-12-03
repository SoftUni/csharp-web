namespace MoiteRecepti.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using MoiteRecepti.Data.Common.Repositories;
    using MoiteRecepti.Data.Models;
    using MoiteRecepti.Services.Mapping;

    public class IngredientsService : IIngredientsService
    {
        private readonly IDeletableEntityRepository<Ingredient> ingredientsRepository;

        public IngredientsService(IDeletableEntityRepository<Ingredient> ingredientsRepository)
        {
            this.ingredientsRepository = ingredientsRepository;
        }

        public IEnumerable<T> GetAllPopular<T>()
        {
            return this.ingredientsRepository.All()
                .Where(x => x.Recipes.Count() >= 20)
                .OrderByDescending(x => x.Recipes.Count())
                .To<T>().ToList();
        }
    }
}
