using SIS.Common;
using SIS.HTTP.Common;

namespace SIS.HTTP.Headers
{
    public class HttpHeader
    {
        public const string Cookie = "Cookie";

        public const string ContentType = "Content-Type";

        public const string ContentLength = "Content-Length";

        public const string ContentDisposition = "Content-Disposition";

        public HttpHeader(string key, string value)
        {
            key.ThrowIfNullOrEmpty(nameof(key));
            value.ThrowIfNullOrEmpty(nameof(value));
            this.Key = key;
            this.Value = value;
        }

        public string Key { get; }

        public string Value { get; }

        public override string ToString() => $"{this.Key}: {this.Value}";
    }
}
