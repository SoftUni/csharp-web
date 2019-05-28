using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.MvcFramework;

namespace IRunes.App.Controllers
{
    public class InfoController : Controller
    {
        public int MyProperty { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }

        public IHttpResponse About(IHttpRequest request)
        {
            return this.View();
        }
    }
}
