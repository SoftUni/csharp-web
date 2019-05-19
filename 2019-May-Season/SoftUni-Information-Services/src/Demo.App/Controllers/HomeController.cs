namespace Demo.App.Controllers
{
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    public class HomeController : BaseController
    {
        public IHttpResponse Home(IHttpRequest httpRequest)
        {
            return this.View();
        }

        public IHttpResponse About(IHttpRequest httpRequest)
        {
            
            return this.View();
        }
    }
}
