using SIS.MvcFramework;
using System;
using System.Globalization;
using System.Threading;

namespace Panda.Web
{
    public static class Program
    {
        public static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            WebHost.Start(new Startup());
        }
    }
}
