using System;
using System.Text;
using SIS.Common;

namespace SIS.HTTP.Cookies
{
    public class HttpCookie
    {
        private const int HttpCookieDefaultExpirationDays = 3;

        private const string HttpCookieDefaultPath = "/";

        public HttpCookie(string key, string value, int expires = HttpCookieDefaultExpirationDays,
            string path = HttpCookieDefaultPath) : this(key, value, true, expires, path)
        {
        }

        public HttpCookie(string key, string value, bool isNew, int expires = HttpCookieDefaultExpirationDays,
            string path = HttpCookieDefaultPath)

        {
            key.ThrowIfNullOrEmpty(nameof(key));
            value.ThrowIfNullOrEmpty(nameof(value));

            this.Key = key;
            this.Value = value;
            this.Expires = DateTime.UtcNow.AddDays(expires);
			this.Path = path;
        }


        public string Key { get; }

        public string Value { get; }

        public DateTime Expires { get; private set; }

        public string Path { get; set; }

        public bool IsNew { get; }

        public bool HttpOnly { get; set; } = true;

        public void Delete()
        {
            this.Expires = DateTime.UtcNow.AddDays(-1);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{this.Key}={this.Value}; Expires={this.Expires:R}");

            if (this.HttpOnly)
            {
                sb.Append("; HttpOnly");
            }

            sb.Append($"; Path={this.Path}");

            return sb.ToString();
        }
    }
}
