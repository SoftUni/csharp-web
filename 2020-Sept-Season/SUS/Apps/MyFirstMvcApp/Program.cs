using SUS.HTTP;
using System.Threading.Tasks;

namespace MyFirstMvcApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // TODO: {controller}/{action}/{id}
            IHttpServer server = new HttpServer();
            server.AddRoute("/", HomePage);
            server.AddRoute("/niki", (request) =>
            {
                return new HttpResponse("text/html", new byte[] { 0x56, 0x57 });
            });
            server.AddRoute("/favicon.ico", Favicon);
            server.AddRoute("/about", About);
            server.AddRoute("/users/login", Login);
            server.AddRoute("/users/register", Register);
            // Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "http://localhost/");
            await server.StartAsync(80);
        }
    }
}
