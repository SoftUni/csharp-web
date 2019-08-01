using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstMvcApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFirstMvcApp.Services;
using MyFirstMvcApp.Filters;
using MyFirstMvcApp.ModelBinders;
using MyFirstMvcAppML.Model.DataModels;
using Microsoft.Extensions.ML;
using MyFirstMvcApp.RouteConstraints;

namespace MyFirstMvcApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddResponseCaching();
            services.AddMemoryCache();
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                options.SchemaName = "dbo";
                options.TableName = "CacheEntries";
            });

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
            });

            services.AddRouting(options =>
            {
                options.ConstraintMap.Add("cyrillic", typeof(CyrillicRouteConstraint));
            });

            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new DateTimeToYearModelBinderProvider());
                options.Filters.Add(new AddHeaderAttribute("X-Debug", "Global!!!"));
                options.Filters.Add(new MyResourceFilter());
                options.Filters.Add(new ValidateModelStateFilterAttribute());
                options.Filters.Add(new MyResultFilter());
                options.Filters.Add(new MyExceptionFilter());
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(new CustomPageFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.AddPredictionEnginePool<ModelInput, ModelOutput>()
                    .FromFile("MLModels/MLModel.zip");

            // Application Services
            services.AddTransient<IUsersService, UsersService>();
            services.AddSingleton<ICounterService, CounterService>();
            services.AddTransient<MyAuthorizeFilterAttribute>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("QA"))
            {
                app.UseDeveloperExceptionPage(new DeveloperExceptionPageOptions() { SourceCodeLineCount = 100 });
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseResponseCaching();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
