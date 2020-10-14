using System;
using System.Collections.Generic;
using System.Text;

namespace SUS.HTTP
{
    public class Route
    {
        public Route(string path, HttpMethod method, Func<HttpRequest, HttpResponse> action, bool IsAuthorized = false)
        {
            this.Path = path;
            this.Method = method;
            this.Action = action;
            this.IsAuthorized = IsAuthorized;
        }

        public string Path { get; set; }

        public HttpMethod Method { get; set; }

        public Func<HttpRequest, HttpResponse> Action { get; set; }

        public bool IsAuthorized  { get; set; }
    }
}
