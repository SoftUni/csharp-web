namespace MoiteRecepti.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MoiteRecepti.Data.Models;
    using MoiteRecepti.Web.ViewModels.Recipes;

    public interface IRecipesService
    {
        Task CreateAsync(CreateRecipeInputModel input, string userId, string imagePath);

        IEnumerable<T> GetAll<T>(IQueryable<Recipe> query, int page, int itemsPerPage = 12);

        IEnumerable<T> GetRandom<T>(int count);

        T GetById<T>(int id);

        Task UpdateAsync(int id, EditRecipeInputModel input);

        IEnumerable<T> GetByIngredients<T>(IEnumerable<int> ingredientIds);

        Task DeleteAsync(int id);
    }
}
