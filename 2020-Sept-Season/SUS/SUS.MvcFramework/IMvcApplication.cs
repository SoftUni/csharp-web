using SUS.HTTP;
using System.Collections.Generic;

namespace SUS.MvcFramework
{
    public interface IMvcApplication
    {
        void ConfigureServices();

        void Configure(List<Route> routeTable);
    }
}
