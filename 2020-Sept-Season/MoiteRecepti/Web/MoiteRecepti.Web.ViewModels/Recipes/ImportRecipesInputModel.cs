namespace MoiteRecepti.Web.ViewModels.Recipes
{
    using System.ComponentModel.DataAnnotations;

    public class ImportRecipesInputModel
    {
        [Range(1, int.MaxValue)]
        public int Count { get; set; }
    }
}
