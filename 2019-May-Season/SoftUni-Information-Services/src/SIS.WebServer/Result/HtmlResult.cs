namespace SIS.WebServer.Result
{
    using System.Text;

    using SIS.HTTP.Common;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Responses.Contracts;
    public class HtmlResult : HttpResponse, IHttpResponse
    {
        private const string TextHtmlContentTypeHeaderValue = "text/html; charset=utf-8";

        public HtmlResult(string content, HttpResponseStatusCode responseStatusCode) 
            : base(responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader(GlobalConstants.ContentTypeHeaderKey, TextHtmlContentTypeHeaderValue));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
