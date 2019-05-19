namespace SIS.HTTP.Responses.Contracts
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers.Contracts;
    public interface IHttpResponse
    {
        HttpResponseStatusCode StatusCode { get; set; }

        IHttpHeaderCollection Headers { get; }

        byte[] Content { get; set; }

        void AddHeader(IHttpHeader header);

        byte[] GetBytes();
    }
}
