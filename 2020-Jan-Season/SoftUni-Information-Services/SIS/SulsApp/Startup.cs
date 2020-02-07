using Microsoft.EntityFrameworkCore;
using SIS.HTTP;
using SIS.HTTP.Logging;
using SIS.MvcFramework;
using SulsApp.Services;
using System;
using System.Collections.Generic;

namespace SulsApp
{
    public class Startup : IMvcApplication
    {
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Add<IUsersService, UsersService>();
        }

        public void Configure(IList<Route> routeTable)
        {
            // Middleware...
            var db = new ApplicationDbContext();
            db.Database.Migrate();
        }
    }
}
