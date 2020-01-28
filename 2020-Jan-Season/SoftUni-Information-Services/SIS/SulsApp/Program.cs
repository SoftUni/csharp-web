using SIS.HTTP;
using SIS.MvcFramework;
using SulsApp.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SulsApp
{
    public static class Program
    {
        public static async Task Main()
        {
            await WebHost.StartAsync(new Startup());
        }
    }
}
