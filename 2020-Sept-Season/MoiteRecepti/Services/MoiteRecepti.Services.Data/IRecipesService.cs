namespace MoiteRecepti.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MoiteRecepti.Web.ViewModels.Recipes;

    public interface IRecipesService
    {
        Task CreateAsync(CreateRecipeInputModel input, string userId);

        IEnumerable<T> GetAll<T>(int page, int itemsPerPage = 12);

        int GetCount();
    }
}
