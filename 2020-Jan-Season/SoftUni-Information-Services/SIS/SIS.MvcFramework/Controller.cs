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
        public HttpRequest Request { get; set; }

        protected HttpResponse View<T>(T viewModel = null, [CallerMemberName]string viewName = null)
            where T : class
        {
            IViewEngine viewEngine = new ViewEngine();
            var typeName = this.GetType().Name/*.Replace("Controller", string.Empty)*/;
            var controllerName = typeName.Substring(0, typeName.Length - 10);
            var html = File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");
            html = viewEngine.GetHtml(html, viewModel);

            var layout = File.ReadAllText("Views/Shared/_Layout.html");
            var bodyWithLayout = layout.Replace("@RenderBody()", html);
            bodyWithLayout = viewEngine.GetHtml(bodyWithLayout, viewModel);
            return new HtmlResponse(bodyWithLayout);
        }
        protected HttpResponse View([CallerMemberName]string viewName = null)
        {
            return this.View<object>(null, viewName);
        }
    }
}
