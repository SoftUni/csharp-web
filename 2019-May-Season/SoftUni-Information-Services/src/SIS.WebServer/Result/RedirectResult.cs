using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.MvcFramework.Result
{
    public class RedirectResult : ActionResult
    {
        public RedirectResult(string location) : base(HttpResponseStatusCode.SeeOther)
        {
            location = !location.StartsWith("/") ? "/" + location : location;
            this.Headers.AddHeader(new HttpHeader("Location", location));
        }
    }
}
