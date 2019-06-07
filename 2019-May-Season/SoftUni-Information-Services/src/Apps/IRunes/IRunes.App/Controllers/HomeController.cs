using IRunes.App.ViewModels;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Result;
using System.Collections.Generic;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet(Url = "/")]
        public IActionResult IndexSlash()
        {
            return Index();
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Test(IEnumerable<string> list)
        {
            return this.View();
        }
    }
}
