using SUS.HTTP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SUS.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(List<Route> routeTable, int port = 80)
        {
            // TODO: {controller}/{action}/{id}
            IHttpServer server = new HttpServer(routeTable);

            // Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "http://localhost/");
            await server.StartAsync(port);
        }
    }
}
