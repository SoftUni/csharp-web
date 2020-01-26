using System;
using System.Collections.Generic;

namespace SIS.HTTP
{
    public class HttpRequest
    {
        public HttpRequest(string httpRequestAsString)
        {
            this.Headers = new List<Header>();
            this.Cookies = new List<Cookie>();
            this.Body = new Dictionary<string, string>();

            this.ParseRequest(httpRequestAsString);
        }

        public HttpMethodType Method { get; set; }

        public string Path { get; set; }

        public HttpVersionType Version { get; set; }

        public IList<Header> Headers { get; set; }

        public IList<Cookie> Cookies { get; set; }

        public IDictionary<string, string> Body { get; set; }

        public IDictionary<string, string> SessionData { get; set; }

        private void ParseInfoHeader(string httpInfoHeader)
        {
            var infoHeaderParts = httpInfoHeader.Split(' ');

            if (infoHeaderParts.Length != 3)
            {
                throw new HttpServerException("Invalid HTTP header line.");
            }

            var httpMethod = infoHeaderParts[0];

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
        }

        private void ParseRequestHeaders(string[] httpLines)
        {
            for (int i = 1; i < httpLines.Length; i++)
            {
                var line = httpLines[i];

                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                var headerParts = line.Split(new string[] { ": " }, 2, StringSplitOptions.None);

                if (headerParts.Length != 2)
                {
                    throw new HttpServerException($"Invalid header: {line}");
                }

                var header = new Header(headerParts[0], headerParts[1]);
                this.Headers.Add(header);

                if (headerParts[0] == "Cookie")
                {
                    var cookiesAsString = headerParts[1];
                    this.ParseCookies(cookiesAsString);
                }
            }
        }

        private void ParseCookies(string cookiesAsString)
        {
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

        private void ParseRequestBody(string bodyLine)
        {
            var bodyKeyValues = bodyLine.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var bodyKeyValue in bodyKeyValues)
            {
                var keyValue = bodyKeyValue.Split('=');

                if (!this.Body.ContainsKey(keyValue[0]))
                {
                    this.Body.Add(keyValue[0], keyValue[1]);
                }
            }
        }

        private void ParseRequest(string httpRequestAsString)
        {
            var lines = httpRequestAsString.Split(
                new string[] { HttpConstants.NewLine },
                StringSplitOptions.None);

            var httpInfoHeader = lines[0];

            this.ParseInfoHeader(httpInfoHeader);
            this.ParseRequestHeaders(lines);

            var indexOfBodyLine = Array.IndexOf(lines, string.Empty) + 1;

            if (!string.IsNullOrEmpty(lines[indexOfBodyLine]))
            {
                var bodyLine = lines[indexOfBodyLine];
                this.ParseRequestBody(bodyLine);
            }
        }
    }
}
