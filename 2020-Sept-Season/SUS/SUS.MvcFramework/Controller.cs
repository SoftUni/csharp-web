using SUS.HTTP;
using SUS.MvcFramework.ViewEngine;
using System.Runtime.CompilerServices;
using System.Text;

namespace SUS.MvcFramework
{
    public abstract class Controller
    {
        private const string UserIdSessionName = "UserId";
        private SusViewEngine viewEngine;

        public Controller()
        {
            this.viewEngine = new SusViewEngine();
        }

        public HttpRequest Request { get; set; }

        protected HttpResponse View(
            object viewModel = null,
            [CallerMemberName]string viewPath = null)
        {
            var viewContent = System.IO.File.ReadAllText(
                "Views/" + 
                this.GetType().Name.Replace("Controller", string.Empty) + 
                "/" + viewPath + ".cshtml");
            viewContent = this.viewEngine.GetHtml(viewContent, viewModel, this.GetUserId());

            var responseHtml = this.PutViewInLayout(viewContent, viewModel);

            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse("text/html", responseBodyBytes);
            return response;
        }

        protected HttpResponse File(string filePath, string contentType)
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var response = new HttpResponse(contentType, fileBytes);
            return response;
        }

        protected HttpResponse Redirect(string url)
        {
            var response = new HttpResponse(HttpStatusCode.Found);
            response.Headers.Add(new Header("Location", url));
            return response;
        }

        protected HttpResponse Error(string errorText)
        {
            var viewContent = $"<div class=\"alert alert-danger\" role=\"alert\">{errorText}</div>";
            var responseHtml = this.PutViewInLayout(viewContent);
            var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
            var response = new HttpResponse("text/html", responseBodyBytes, HttpStatusCode.ServerError);
            return response;
        }

        protected void SignIn(string userId)
        {
            this.Request.Session[UserIdSessionName] = userId;
        }

        protected void SignOut()
        {
            this.Request.Session[UserIdSessionName] = null;
        }

        protected bool IsUserSignedIn() =>
            this.Request.Session.ContainsKey(UserIdSessionName) &&
            this.Request.Session[UserIdSessionName] != null;

        protected string GetUserId() =>
            this.Request.Session.ContainsKey(UserIdSessionName) ?
            this.Request.Session[UserIdSessionName] : null;

        private string PutViewInLayout(string viewContent, object viewModel = null)
        {
            var layout = System.IO.File.ReadAllText("Views/Shared/_Layout.cshtml");
            layout = layout.Replace("@RenderBody()", "____VIEW_GOES_HERE____");
            layout = this.viewEngine.GetHtml(layout, viewModel, this.GetUserId());
            var responseHtml = layout.Replace("____VIEW_GOES_HERE____", viewContent);
            return responseHtml;
        }
    }
}
