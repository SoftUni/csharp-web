namespace MoiteRecepti.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MoiteRecepti.Web.ViewModels.Recipes;

    public interface IRecipesService
    {
        Task CreateAsync(CreateRecipeInputModel input, string userId, string imagePath);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12);

        IEnumerable<T> GetRandom<T>(int count);

        int GetCount();

        T GetById<T>(int id);

        Task UpdateAsync(int id, EditRecipeInputModel input);

        IEnumerable<T> GetByIngredients<T>(IEnumerable<int> ingredientIds);

        Task DeleteAsync(int id);
    }
}
