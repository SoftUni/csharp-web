using System.Runtime.CompilerServices;
using SIS.HTTP.Requests;
using SIS.MvcFramework.Extensions;
using SIS.MvcFramework.Identity;
using SIS.MvcFramework.Result;
using SIS.MvcFramework.ViewEngine;

namespace SIS.MvcFramework
{
    using Validation;

    public abstract class Controller
    {
        private readonly IViewEngine viewEngine;

        protected Controller()
        {
            this.viewEngine = new SisViewEngine();
            this.ModelState = new ModelStateDictionary();
        }

        // TODO: Refactor this
        public Principal User => 
            this.Request.Session.ContainsParameter("principal")
            ? (Principal) this.Request.Session.GetParameter("principal")
            : null;

        public IHttpRequest Request { get; set; }

        public ModelStateDictionary ModelState { get; set; }

        protected bool IsLoggedIn()
        {
            return this.Request.Session.ContainsParameter("principal");
        }

        protected void SignIn(string id, string username, string email)
        {
            this.Request.Session.AddParameter("principal", new Principal
            {
                Id = id,
                Username = username,
                Email = email
            });
        }

        protected void SignOut()
        {
            this.Request.Session.ClearParameters();
        }

        protected ActionResult View([CallerMemberName] string view = null)
        {
            return this.View<object>(null, view);
        }

        protected ActionResult View<T>(T model = null, [CallerMemberName] string view = null)
            where T : class
        {
            // TODO: Support for layout
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string viewName = view;

            string viewContent = System.IO.File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");
            viewContent = this.viewEngine.GetHtml(viewContent, model,this.ModelState, this.User);

            string layoutContent = System.IO.File.ReadAllText("Views/_Layout.html");
            layoutContent = this.viewEngine.GetHtml(layoutContent, model,this.ModelState, this.User);
            layoutContent = layoutContent.Replace("@RenderBody()", viewContent);

            var htmlResult = new HtmlResult(layoutContent);
            return htmlResult;
        }

        protected ActionResult Redirect(string url)
        {
            return new RedirectResult(url);
        }

        protected ActionResult Xml(object obj)
        {
            return new XmlResult(obj.ToXml());
        }

        protected ActionResult Json(object obj)
        {
            return new JsonResult(obj.ToJson());
        }

        protected ActionResult File(byte[] fileContent)
        {
            return new FileResult(fileContent);
        }

        protected ActionResult NotFound(string message = "")
        {
            return new NotFoundResult(message);
        }
    }
}
