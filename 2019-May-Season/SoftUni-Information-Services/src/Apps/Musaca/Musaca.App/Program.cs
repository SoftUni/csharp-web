using SIS.MvcFramework;

namespace Musaca.App
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost.Start(new Startup());
        }
    }
}
