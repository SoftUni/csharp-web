using System;
using System.Threading.Tasks;

namespace SIS.HTTP
{
    public class Route
    {
        public Route(HttpMethodType httpMethod, string path, Func<HttpRequest, HttpResponse> action)
        {
            HttpMethod = httpMethod;
            Path = path;
            Action = action;
        }

        public Route(HttpMethodType httpMethod, string path, Func<HttpRequest, Task<HttpResponse>> asyncAction)
        {
            HttpMethod = httpMethod;
            Path = path;
            AsyncAction = asyncAction;
        }

        public string Path { get; set; }

        public HttpMethodType HttpMethod { get; set; }

        public Func<HttpRequest, HttpResponse> Action { get; set; }

        public Func<HttpRequest, Task<HttpResponse>> AsyncAction { get; set; }
    }
}
