using System.Collections.Generic;
using System.Linq;
using SIS.Common;
using SIS.HTTP.Common;

namespace SIS.HTTP.Headers
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private Dictionary<string, HttpHeader> httpHeaders;

        public HttpHeaderCollection()
        {
            this.httpHeaders = new Dictionary<string, HttpHeader>();
        }

        public void AddHeader(HttpHeader header)
        {
            header.ThrowIfNull(nameof(header));
            this.httpHeaders.Add(header.Key, header);
        }

        public bool ContainsHeader(string key)
        {
            key.ThrowIfNullOrEmpty(nameof(key));
            return this.httpHeaders.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            key.ThrowIfNullOrEmpty(nameof(key));
            return this.httpHeaders[key];
        }

        public override string ToString() => string.Join("\r\n",
            this.httpHeaders.Values.Select(header => header.ToString()));

    }
}