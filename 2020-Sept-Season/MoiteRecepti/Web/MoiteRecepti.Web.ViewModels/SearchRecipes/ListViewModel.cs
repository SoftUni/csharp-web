namespace MoiteRecepti.Web.ViewModels.SearchRecipes
{
    using System.Collections.Generic;

    using MoiteRecepti.Web.ViewModels.Recipes;

    public class ListViewModel
    {
        public IEnumerable<RecipeInListViewModel> Recipes { get; set; }
    }
}
