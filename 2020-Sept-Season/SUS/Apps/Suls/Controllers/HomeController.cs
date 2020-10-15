using Suls.ViewModels.Problems;
using SUS.HTTP;
using SUS.MvcFramework;
using System.Collections.Generic;

namespace Suls.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (this.IsUserSignedIn())
            {
                return this.View(new List<HomePageProblemViewModel>(), "IndexLoggedIn");
            }
            else
            {
                return this.View();
            }
        }
    }
}
