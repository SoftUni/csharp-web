using SIS.HTTP;
using System;
using System.Threading.Tasks;

namespace DemoApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Actions:
            // / => response IndexPage(request)
            // /favicon.ico => favicon.ico
            // GET /Contact => response ShowContactForm(request)
            // POST /Contact => reponse FillContactForm(request)

            var httpServer = new HttpServer(80);
            await httpServer.StartAsync();
        }
    }
}
