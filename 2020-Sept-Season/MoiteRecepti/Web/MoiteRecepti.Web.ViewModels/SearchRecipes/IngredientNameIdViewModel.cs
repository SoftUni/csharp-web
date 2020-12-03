namespace MoiteRecepti.Web.ViewModels.SearchRecipes
{
    using MoiteRecepti.Data.Models;
    using MoiteRecepti.Services.Mapping;

    public class IngredientNameIdViewModel : IMapFrom<Ingredient>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
