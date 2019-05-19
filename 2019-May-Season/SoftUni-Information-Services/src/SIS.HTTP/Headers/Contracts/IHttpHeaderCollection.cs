namespace SIS.HTTP.Headers.Contracts
{
    public interface IHttpHeaderCollection
    {
        void AddHeader(IHttpHeader header);

        bool ContainsHeader(string key);

        IHttpHeader GetHeader(string key);
    }
}
