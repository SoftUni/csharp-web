namespace SIS.HTTP
{
    using System;
    using System.Web;
    using System.Text;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an HTTP Request with properties for the <c>Request Line</c>, <c>Request Headers</c> and <c>Request Body</c>.
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// Initializes a new <see cref="HttpRequest"/> class.
        /// </summary>
        /// <param name="httpRequestAsString">HTTP Request string</param>
        public HttpRequest(string httpRequestAsString)
        {
            this.Headers = new List<Header>();
            this.Cookies = new List<Cookie>();

            var lines = httpRequestAsString.Split(
                new string[] { HttpConstants.NewLine },
                StringSplitOptions.None);
            var httpInfoHeader = lines[0];
            var infoHeaderParts = httpInfoHeader.Split(' ');
            if (infoHeaderParts.Length != 3)
            {
                throw new HttpServerException("Invalid HTTP header line.");
            }

            var httpMethod = infoHeaderParts[0];
            // Enum.TryParse(httpMethod, out HttpMethodType type);
            this.Method = httpMethod switch
            {
                "GET" => HttpMethodType.Get,
                "POST" => HttpMethodType.Post,
                "PUT" => HttpMethodType.Put,
                "DELETE" => HttpMethodType.Delete,
                _ => HttpMethodType.Unknown,
            };

            this.Path = infoHeaderParts[1];

            var httpVersion = infoHeaderParts[2];
            this.Version = httpVersion switch
            {
                "HTTP/1.0" => HttpVersionType.Http10,
                "HTTP/1.1" => HttpVersionType.Http11,
                "HTTP/2.0" => HttpVersionType.Http20,
                _ => HttpVersionType.Http11,
            };

            bool isInHeader = true;
            StringBuilder bodyBuilder = new StringBuilder();
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeader = false;
                    continue;
                }

                if (isInHeader)
                {
                    var headerParts = line.Split(
                        new string[] { ": " },
                        2,
                        StringSplitOptions.None);
                    if (headerParts.Length != 2)
                    {
                        throw new HttpServerException($"Invalid header: {line}");
                    }

                    var header = new Header(headerParts[0], headerParts[1]);
                    this.Headers.Add(header);

                    if (headerParts[0] == "Cookie")
                    {
                        var cookiesAsString = headerParts[1];
                        var cookies = cookiesAsString.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var cookieAsString in cookies)
                        {
                            var cookieParts = cookieAsString.Split(new char[] { '=' }, 2);
                            if (cookieParts.Length == 2)
                            {
                                this.Cookies.Add(new Cookie(cookieParts[0], cookieParts[1]));
                            }
                        }
                    }
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }
            }

            // creator=Niki&tweetName=Hello!
            this.Body = bodyBuilder.ToString().TrimEnd('\r', '\n');
            this.FormData = new Dictionary<string, string>();
            ParseData(this.FormData, this.Body);

            this.Query = string.Empty;
            if (this.Path.Contains("?"))
            {
                var parts = this.Path.Split(new char[] { '?' }, 2);
                this.Path = parts[0];
                this.Query = parts[1];
            }

            this.QueryData = new Dictionary<string, string>();
            ParseData(this.QueryData, this.Query);
        }

        private void ParseData(IDictionary<string, string> output, string input)
        {
            var dataParts = input.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dataPart in dataParts)
            {
                var parameterParts = dataPart.Split(new char[] { '=' }, 2);
                output.Add(
                    HttpUtility.UrlDecode(parameterParts[0]),
                    HttpUtility.UrlDecode(parameterParts[1]));
            }
        }

        /// <summary>
        /// HTTP Request line Method.
        /// </summary>
        public HttpMethodType Method { get; set; }

        /// <summary>
        /// HTTP Request line Path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// HTTP Request line Version.
        /// </summary>
        public HttpVersionType Version { get; set; }

        /// <summary>
        /// Collection of HTTP Request Headers.
        /// </summary>
        public IList<Header> Headers { get; set; }

        /// <summary>
        /// Collection of HTTP Request Cookies.
        /// </summary>
        public IList<Cookie> Cookies { get; set; }

        /// <summary>
        /// HTTP Request Body.
        /// </summary>
        public string Body { get; set; }

        public IDictionary<string, string> FormData { get; set; }

        public string Query { get; set; }

        public IDictionary<string, string> QueryData { get; set; }

        public IDictionary<string, string> SessionData { get; set; }
    }
}
