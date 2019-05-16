using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Result;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Demo.App.Controllers
{
    public abstract class BaseController
    {
        public IHttpResponse View([CallerMemberName] string view = null)
        {
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;

            string viewContent = File.ReadAllText("Views" + controllerName + "/" + viewName + ".html");

            return new HtmlResult(viewContent, SIS.HTTP.Enums.HttpResponseStatusCode.Ok);
        }
    }
}
