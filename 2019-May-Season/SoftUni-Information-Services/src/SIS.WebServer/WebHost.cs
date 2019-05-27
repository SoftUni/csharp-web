using SIS.WebServer;
using SIS.WebServer.Routing;

namespace SIS.MvcFramework
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            var serverRoutingTable = new ServerRoutingTable();
            application.ConfigureServices();
            application.Configure(serverRoutingTable);
            var server = new Server(8000, serverRoutingTable);
            server.Run();
        }
    }
}
