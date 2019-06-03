using IRunes.Data;

using SIS.MvcFramework;
using SIS.MvcFramework.Routing;

namespace IRunes.App
{
    public class Startup : IMvcApplication
    {
        public void Configure(IServerRoutingTable serverRoutingTable)
        {
            using (var context = new RunesDbContext())
            {
                context.Database.EnsureCreated();
            }
        }

        public void ConfigureServices()
        {
        }
    }
}
