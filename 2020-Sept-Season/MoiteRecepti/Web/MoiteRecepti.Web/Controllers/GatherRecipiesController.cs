using Microsoft.AspNetCore.Mvc;
using MoiteRecepti.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoiteRecepti.Web.Controllers
{
    // TODO: Move in administration area
    public class GatherRecipiesController : BaseController
    {
        private readonly IGotvachBgScraperService gotvachBgScraperService;

        public GatherRecipiesController(IGotvachBgScraperService gotvachBgScraperService)
        {
            this.gotvachBgScraperService = gotvachBgScraperService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Add()
        {
            await this.gotvachBgScraperService.PopulateDbWithRecipesAsync(300);

            return this.RedirectToAction("Index");
        }
    }
}
