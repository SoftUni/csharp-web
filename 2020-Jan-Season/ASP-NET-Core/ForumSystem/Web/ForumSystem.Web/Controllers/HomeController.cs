namespace ForumSystem.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using ForumSystem.Data;
    using ForumSystem.Services.Messaging;
    using ForumSystem.Web.ViewModels;
    using ForumSystem.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel();
            var categories = this.db.Categories.Select(x => new IndexCategoryViewModel
            {
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Name = x.Name,
                Title = x.Title,
            }).ToList();
            viewModel.Categories = categories;

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
