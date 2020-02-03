namespace SIS.HTTP
{
    using System;
    using System.Text;

    /// <summary>
    /// Represents an HTTP Response Cookie with information for Cookie Scope, Cookie Lifetime, Cookie Security and SameSite.
    /// </summary>
    public class ResponseCookie : Cookie
    {
        //------------- CONSTRUCTORS -------------
        /// <summary>
        /// Initializes a new <see cref="ResponseCookie"/> class.
        /// </summary>
        /// <param name="name">Cookie name</param>
        /// <param name="value">Cookie value</param>
        public ResponseCookie(string name, string value)
            : base(name, value)
        {
            this.Path = "/";
            this.SameSite = SameSiteType.None;
            this.Expires = DateTime.UtcNow.AddDays(30);
        }

        //-------------- PROPERTIES --------------
        /// <summary>
        /// Cookie scope attribute - <c>Domain</c>.
        /// Specifies allowed hosts to receive the cookie.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Cookie scope attribute - <c>Path</c>.
        /// Indicates a URL path that must exist in the requested URL in order to send the Cookie header.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Cookie lifetime attribute - <c>Expires</c>.
        /// Sets an expiry date for when a cookie gets deleted.
        /// </summary>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Cookie lifetime attribute - <c>Max-Age</c>.
        /// Sets the time in seconds for when a cookie will be deleted.
        /// </summary>
        public long? MaxAge { get; set; }

        /// <summary>
        /// Cookie security attribute - <c>Secure</c> flag.
        /// The secure flag is an option that can be set by the application server when sending a new cookie to the user within an HTTP Response.
        /// The purpose of the secure flag is to prevent cookies from being observed by unauthorized parties due to the transmission of a the cookie in clear text.
        /// To accomplish this goal, browsers which support the secure flag will only send cookies with the secure flag when the request is going to a HTTPS page.
        /// Said in another way, the browser will not send a cookie with the secure flag set over an unencrypted HTTP request.
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        /// Cookie security attribute - <c>HttpOnly</c> flag.
        /// Helps mitigate the risk of client side script accessing the protected cookie.
        /// If the <c>HttpOnly</c> flag is included in the HTTP response header, the cookie cannot be accessed through client side script
        /// </summary>
        public bool HttpOnly { get; set; }

        /// <summary>
        /// Cookie security attribute - <c>SameSite</c>. 
        /// SameSite prevents the browser from sending this cookie along with cross-site requests. 
        /// The main goal is mitigate the risk of cross-origin information leakage. It also provides some protection against cross-site request forgery attacks.
        /// </summary>
        public SameSiteType SameSite { get; set; }

        //------------ PUBLIC METHODS ------------
        /// <summary>
        /// Returns formatted HTTP Response Cookie.
        /// </summary>
        /// <returns>Formatted HTTP Response Cookie.</returns>
        public override string ToString()
        {
            StringBuilder cookieBuilder = new StringBuilder();
            cookieBuilder.Append($"{this.Name}={this.Value}");
            if (this.MaxAge.HasValue)
            {
                cookieBuilder.Append($"; Max-Age=" + this.MaxAge.Value.ToString());
            }
            else if (this.Expires.HasValue)
            {
                cookieBuilder.Append($"; Expires=" + this.Expires.Value.ToString("R"));
            }

            if (!string.IsNullOrWhiteSpace(this.Domain))
            {
                cookieBuilder.Append($"; Domain=" + this.Domain);
            }

            if (!string.IsNullOrWhiteSpace(this.Path))
            {
                cookieBuilder.Append($"; Path=" + this.Path);
            }

            if (this.Secure)
            {
                cookieBuilder.Append("; Secure");
            }

            if (this.HttpOnly)
            {
                cookieBuilder.Append("; HttpOnly");
            }

            cookieBuilder.Append("; SameSite=" + this.SameSite.ToString());

            return cookieBuilder.ToString();
        }
    }
}
