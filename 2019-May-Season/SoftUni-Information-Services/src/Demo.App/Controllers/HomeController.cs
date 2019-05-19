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
            foreach (var item in httpRequest.Headers)
            {
                System.Console.WriteLine(item);
            }

            System.Console.WriteLine(httpRequest.Headers["Host"]);
            return this.View();
        }
    }
}
