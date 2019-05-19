namespace Demo.App.Controllers
{
    using System.IO;
    using System.Runtime.CompilerServices;

    using SIS.HTTP.Enums;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Result;
    public abstract class BaseController
    {
        public IHttpResponse View([CallerMemberName] string view = null)
        {
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string viewName = view; 

            string viewContent = File.ReadAllText("Views" + "/" + controllerName + "/" + viewName + ".html");

            return new HtmlResult(viewContent, HttpResponseStatusCode.Ok);
        }
    }
}
