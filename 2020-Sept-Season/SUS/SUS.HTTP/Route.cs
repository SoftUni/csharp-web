using System;
using System.Collections.Generic;
using System.Text;

namespace SUS.HTTP
{
    public class Route
    {
        public Route(string path, HttpMethod method, Func<HttpRequest, HttpResponse> action)
        {
            this.Path = path;
            this.Method = method;
            this.Action = action;
        }

        public string Path { get; set; }

        public HttpMethod Method { get; set; }

        public Func<HttpRequest, HttpResponse> Action { get; set; }
    }
}
