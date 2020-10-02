using SUS.MvcFramework;
using System.Threading.Tasks;

namespace MyFirstMvcApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // TODO: <Startup>
            await Host.CreateHostAsync(new Startup(), 80);
        }
    }
}
