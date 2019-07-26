using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stopify.Data;
using Stopify.Data.Models;
using System.Linq;
using Stopify.Services;
using System.Globalization;
using CloudinaryDotNet;
using Stopify.Services.Mapping;
using Stopify.Web.InputModels;
using System.Reflection;
using Stopify.Web.ViewModels.Home.Index;
using Stopify.Services.Models;

namespace Stopify.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StopifyDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<StopifyUser, IdentityRole>()
                .AddEntityFrameworkStores<StopifyDbContext>()
                .AddDefaultTokenProviders();

            Account cloudinaryCredentials = new Account(
                this.Configuration["Cloudinary:CloudName"],
                this.Configuration["Cloudinary:ApiKey"],
                this.Configuration["Cloudinary:ApiSecret"]);

            Cloudinary cloudinaryUtility = new Cloudinary(cloudinaryCredentials);

            services.AddSingleton(cloudinaryUtility);

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;

                options.User.RequireUniqueEmail = true;
            });

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IReceiptService, ReceiptService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ProductCreateInputModel).GetTypeInfo().Assembly,
                typeof(ProductHomeViewModel).GetTypeInfo().Assembly,
                typeof(ProductServiceModel).GetTypeInfo().Assembly);

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                using(var context = serviceScope.ServiceProvider.GetRequiredService<StopifyDbContext>())
                {
                    context.Database.EnsureCreated();

                    if(!context.Roles.Any())
                    {
                        context.Roles.Add(new IdentityRole
                        {
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        });

                        context.Roles.Add(new IdentityRole
                        {
                            Name = "User",
                            NormalizedName = "USER"
                        });

                        context.SaveChanges();
                    }

                    if(!context.OrderStatuses.Any())
                    {
                        context.OrderStatuses.Add(new OrderStatus
                        {
                            Name = "Active"
                        });

                        context.OrderStatuses.Add(new OrderStatus
                        {
                            Name = "Completed"
                        });

                        context.SaveChanges();
                    }
                }
            }

            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
