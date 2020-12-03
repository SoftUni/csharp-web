namespace MoiteRecepti.Web.ViewModels.SearchRecipes
{
    using System.Collections.Generic;

    public class SearchIndexViewModel
    {
        public IEnumerable<IngredientNameIdViewModel> Ingredients { get; set; }
    }
}
