namespace SIS.HTTP.Headers
{
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;

    using SIS.HTTP.Common;
    using SIS.HTTP.Headers.Contracts;
    public class HttpHeaderCollection : IHttpHeaderCollection, IEnumerable<IHttpHeader>
    {
        private readonly IList<IHttpHeader> httpHeaders;

        public HttpHeaderCollection()
        {
            this.httpHeaders = new List<IHttpHeader>();
        }

        public void AddHeader(IHttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));
            this.httpHeaders.Add(header);
        }


        public bool ContainsHeader(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            return this.httpHeaders.Any(h => h.Key == key);
        }

        public IEnumerator<IHttpHeader> GetEnumerator()
        {
            return this.httpHeaders.GetEnumerator();
        }

        public IHttpHeader GetHeader(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            return this.httpHeaders.FirstOrDefault(h => h.Key == key);
        }

        public override string ToString() => string.Join("\r\n",
            this.httpHeaders.ToString());

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}