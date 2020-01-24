using SIS.HTTP;
using SIS.HTTP.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DemoApp
{
    public static class Program
    {
        public static async Task Main()
        {
            var routeTable = new List<Route>();
            routeTable.Add(new Route(HttpMethodType.Get, "/", Index));
            routeTable.Add(new Route(HttpMethodType.Get, "/users/login", Login));
            routeTable.Add(new Route(HttpMethodType.Post, "/users/login", DoLogin));
            routeTable.Add(new Route(HttpMethodType.Get, "/contact", Contact));
            routeTable.Add(new Route(HttpMethodType.Get, "/favicon.ico", FavIcon));
            
            var httpServer = new HttpServer(80, routeTable);
            await httpServer.StartAsync();
        }

        // /headers => html table the list of all header

        private static HttpResponse FavIcon(HttpRequest request)
        {
            var byteContent = File.ReadAllBytes("wwwroot/favicon.ico");
            return new FileResponse(byteContent, "image/x-icon");
        }

        private static HttpResponse Contact(HttpRequest request)
        {
            return new HtmlResponse("<h1>contact</h1>");
        }

        public static HttpResponse Index(HttpRequest request)
        {
            var username = request.SessionData.ContainsKey("Username") ? request.SessionData["Username"] : "Anonymous";
            return new HtmlResponse($"<h1>Home page. Hello, {username}</h1><img src='/images/img.jpeg' /><a href='/users/login/'>Go to login</a>");
        }

        public static HttpResponse Login(HttpRequest request)
        {
            request.SessionData["Username"] = "Pesho";
            return new HtmlResponse("<h1>login page</h1>");
        }

        public static HttpResponse DoLogin(HttpRequest request)
        {
            return new HtmlResponse("<h1>login page form</h1>");
        }
    }
}
