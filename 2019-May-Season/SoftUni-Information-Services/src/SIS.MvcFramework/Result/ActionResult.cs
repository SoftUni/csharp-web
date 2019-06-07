using SIS.HTTP.Enums;
using SIS.HTTP.Responses;

namespace SIS.MvcFramework.Result
{
    public abstract class ActionResult : HttpResponse, IActionResult
    {
        protected ActionResult(HttpResponseStatusCode httpResponseStatusCode) : base(httpResponseStatusCode)
        {
        }
    }
}
