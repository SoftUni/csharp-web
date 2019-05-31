using SIS.WebServer.Routing;

namespace SIS.MvcFramework
{
    public interface IMvcApplication
    {
        void Configure(IServerRoutingTable serverRoutingTable);

        void ConfigureServices(); // DI
    }
}
