using Demo.App.Controllers;
using Demo.Data;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Routing;
using SIS.WebServer.Routing.Contracts;

namespace Demo.App
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new DemoDbContext())
            {
                context.Database.EnsureCreated();
            }

            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            // [GET] MAPPINGS
            serverRoutingTable.Add(HttpRequestMethod.Get, "/", httpRequest 
                => new HomeController(httpRequest).Index(httpRequest));
            serverRoutingTable.Add(HttpRequestMethod.Get, "/login", httpRequest
                => new UsersController().Login(httpRequest));
            serverRoutingTable.Add(HttpRequestMethod.Get, "/register", httpRequest
                => new UsersController().Register(httpRequest));
            serverRoutingTable.Add(HttpRequestMethod.Get, "/logout", httpRequest
                => new UsersController().Logout(httpRequest));

            serverRoutingTable.Add(HttpRequestMethod.Get, "/home", httpRequest
                => new HomeController(httpRequest).Home(httpRequest));

            // [POST] MAPPINGS
            serverRoutingTable.Add(HttpRequestMethod.Post, "/login", httpRequest
                => new UsersController().LoginConfirm(httpRequest));
            serverRoutingTable.Add(HttpRequestMethod.Post, "/register", httpRequest
                => new UsersController().RegisterConfirm(httpRequest));

            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }
    }
}
