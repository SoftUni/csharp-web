namespace Demo.App
{
    using Demo.App.Controllers;
    using SIS.HTTP.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Routing;
    using SIS.WebServer.Routing.Contracts;
    public class Program
    {
        public static void Main()
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Add(HttpRequestMethod.Get, "/", httpRequest 
                => new HomeController().Home(httpRequest));

            serverRoutingTable.Add(HttpRequestMethod.Get, "/About", httpRequest
                => new HomeController().About(httpRequest));

            Server server = new Server(8000, serverRoutingTable);
            server.Run();
        }
    }
}
