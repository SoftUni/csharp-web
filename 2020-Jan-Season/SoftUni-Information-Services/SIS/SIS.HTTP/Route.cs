namespace SIS.HTTP
{
    using System;

    /// <summary>
    /// Provides properties and methods for defining a route and for obtaining information about the route.
    /// It enables you to specify how routing is processed in the application.
    /// You create a Route object for each URL pattern that you want to map to a class that can handle requests that correspond to that pattern. 
    /// You then add the route to the Routes collection. 
    /// When the application receives a request, it iterates through the routes in the Routes collection to find the first route that matches the pattern of the URL.
    /// </summary>
    public class Route
    {
        //------------- CONSTRUCTORS -------------
        /// <summary>
        /// Initializes a new <see cref="Route"/> class.
        /// </summary>
        /// <param name="httpMethod">HTTP Method Type</param>
        /// <param name="path">URL Path</param>
        /// <param name="action">Routing function</param>
        public Route(HttpMethodType httpMethod, string path, Func<HttpRequest, HttpResponse> action)
        {
            HttpMethod = httpMethod;
            Path = path;
            Action = action;
        }

        /// <summary>
        /// Route URL Path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// HTTP Method Type.
        /// </summary>
        public HttpMethodType HttpMethod { get; set; }

        /// <summary>
        /// Routing function that receives <see cref="HttpRequest"/> and returns <see cref="HttpResponse"/>.
        /// </summary>
        public Func<HttpRequest, HttpResponse> Action { get; set; }

        public override string ToString()
        {
            return $"{this.HttpMethod} => {this.Path}";
        }
    }
}
