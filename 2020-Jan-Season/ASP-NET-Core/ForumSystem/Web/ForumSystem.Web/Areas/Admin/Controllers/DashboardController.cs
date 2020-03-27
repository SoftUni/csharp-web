namespace ForumSystem.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
