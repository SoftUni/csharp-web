using SIS.HTTP;
using SIS.HTTP.Response;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace SIS.MvcFramework
{
    public abstract class Controller
    {
        protected HttpResponse View([CallerMemberName]string viewName = null)
        {
            var layout = File.ReadAllText("Views/Shared/_Layout.html");
            var typeName = this.GetType().Name/*.Replace("Controller", string.Empty)*/;
            var controllerName = typeName.Substring(0, typeName.Length - 10);
            var html = File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");
            var bodyWithLayout = layout.Replace("@RenderBody()", html);
            return new HtmlResponse(bodyWithLayout);
        }

        protected HttpResponse CssFileView(string fileName)
        {
            var fileContent = File.ReadAllBytes("wwwroot/css/" + $"{fileName}" + ".css");

            return new FileResponse(fileContent, "text/css");
        }
    }
}
