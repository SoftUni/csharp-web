namespace SIS.HTTP.Headers.Contracts
{
    using System.Collections.Generic;
    public interface IHttpHeaderCollection : IEnumerable<IHttpHeader>
    {
        void AddHeader(IHttpHeader header);

        bool ContainsHeader(string key);

        IHttpHeader GetHeader(string key);

        string this[string key] { get; set; }
    }
}
