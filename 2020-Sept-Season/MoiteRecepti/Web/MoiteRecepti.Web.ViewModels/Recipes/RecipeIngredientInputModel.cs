using System.ComponentModel.DataAnnotations;

namespace MoiteRecepti.Web.ViewModels.Recipes
{
    public class RecipeIngredientInputModel
    {
        [Required]
        [MinLength(3)]
        public string IngredientName { get; set; }

        [Required]
        [MinLength(3)]
        public string Quantity { get; set; }
    }
}
