namespace SIS.WebServer.Result
{
    using SIS.HTTP.Common;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Responses.Contracts;
    public class RedirectResult : HttpResponse, IHttpResponse
    {
        public RedirectResult(string location) 
            : base(HttpResponseStatusCode.SeeOther)
        {
            this.Headers.AddHeader(new HttpHeader(GlobalConstants.LocationHeaderKey, location));
        }
    }
}
