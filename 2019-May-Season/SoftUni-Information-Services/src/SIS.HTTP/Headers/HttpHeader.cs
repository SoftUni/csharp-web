namespace SIS.HTTP.Headers
{
    using SIS.HTTP.Common;
    using SIS.HTTP.Headers.Contracts;
    public class HttpHeader : IHttpHeader
    {
        public HttpHeader(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.Key = key;
            this.Value = value;
        }

        public string Key { get; }

        public string Value { get; }

        public override string ToString() => $"{this.Key}: {this.Value}";
    }
}
