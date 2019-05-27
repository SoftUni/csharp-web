using SIS.WebServer.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{
    public interface IMvcApplication
    {
        void Configure(ServerRoutingTable serverRoutingTable);

        void ConfigureServices(); // DI
    }
}
