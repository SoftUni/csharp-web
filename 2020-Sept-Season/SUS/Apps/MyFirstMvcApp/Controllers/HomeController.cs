using SUS.HTTP;
using SUS.MvcFramework;
using System.Linq;
using System.Text;

namespace MyFirstMvcApp.Controllers
{
    public class HomeController : Controller
    {
        public HttpResponse Index(HttpRequest request)
        {
            return this.View();
        }
    }
}
