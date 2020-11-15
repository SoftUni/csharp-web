namespace MoiteRecepti.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using MoiteRecepti.Services;
    using MoiteRecepti.Web.ViewModels.Recipes;

    // TODO: Move in administration area
    public class ImportRecipesController : BaseController
    {
        private readonly IGotvachBgScraperService gotvachBgScraperService;

        public ImportRecipesController(IGotvachBgScraperService gotvachBgScraperService)
        {
            this.gotvachBgScraperService = gotvachBgScraperService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ImportRecipesInputModel model)
        {
            await this.gotvachBgScraperService.ImportRecipesAsync(model.Count);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
