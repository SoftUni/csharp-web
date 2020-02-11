using SIS.MvcFramework;
using System;
using System.Threading.Tasks;

namespace IRunes
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await WebHost.StartAsync(new Startup());
        }
    }
}
