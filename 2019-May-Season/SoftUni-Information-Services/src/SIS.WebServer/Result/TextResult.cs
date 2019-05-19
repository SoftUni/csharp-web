namespace SIS.WebServer.Result
{
    using System.Text;

    using SIS.HTTP.Common;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Responses.Contracts;
    public class TextResult : HttpResponse, IHttpResponse
    {
        private const string TextPlainContentTypeHeaderValue = "text/plain; charset=utf-8";

        public TextResult(string content, HttpResponseStatusCode responseStatusCode) 
            : base(responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader(GlobalConstants.ContentTypeHeaderKey, TextPlainContentTypeHeaderValue));
            this.Content = Encoding.UTF8.GetBytes(content);
        }

        public TextResult(byte[] content, HttpResponseStatusCode responseStatusCode) 
            : base(responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader(GlobalConstants.ContentTypeHeaderKey, TextPlainContentTypeHeaderValue));
            this.Content = content;
        }
    }
}
