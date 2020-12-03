using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using MyFirstAspNetCoreApplication.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFirstAspNetCoreApplication.Service;
using System.Net.Http;
using MyFirstAspNetCoreApplication.Filters;
using MyFirstAspNetCoreApplication.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using MyFirstAspNetCoreApplication.Models;
using MyFirstAspNetCoreApplication.Settings;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MyFirstAspNetCoreApplication
{
    public class Startup
    {
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettingsSection = Configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettingsSection);
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddMemoryCache();
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                options.SchemaName = "dbo";
                options.TableName = "CacheRecords";
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = new TimeSpan(365, 0, 0, 0);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Configure JWT authentication
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 6;
                    if (this.env.IsProduction())
                    {
                        options.Password.RequiredLength = 10;
                    }
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminUserWithState",
                    policy => policy.RequireRole("Admin").RequireClaim("state"));
            });
            services.AddAuthentication(
                options =>
                {
                    // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddFacebook(options =>
            {
                options.AppId = "1231231";
                options.AppSecret = "2312312312";
            }).AddJwtBearer(options => {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddControllersWithViews(configure =>
            {
                configure.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                configure.Filters.Add(new MyAuthFilter());
                configure.Filters.Add(new MyResultFilterAttribute());
                configure.Filters.Add(new MyExceptionFilter());
                configure.Filters.Add(new MyResourceFilter());
                configure.ModelBinderProviders.Insert(0, new ExtractYearModelBinderProvider());
            }).AddXmlSerializerFormatters();
            services.AddRazorPages();
            services.AddCors();

            // singleton / scoped / transient
            services.AddTransient<IInstanceCounter, InstanceCounter>();
            services.AddSingleton<AddHeaderActionFilterAttribute>();
            services.AddTransient<IShortStringService, ShortStringService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var userManager = 
                app.ApplicationServices.CreateScope()
                    .ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (!userManager.Users.Any(x => x.UserName == "kostov@nikolay.it"))
            {
                userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "kostov@nikolay.it",
                    Email = "kostov@nikolay.it",
                    DateOfBirth = DateTime.UtcNow,
                    EmailConfirmed = true,
                }, "kostov@nikolay.it").GetAwaiter().GetResult();
            }

            // Action<HttpContext, RequestDelegate> RequestDelegate
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "areaRoute",
                    "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "blog",
                    pattern: "blog/{year}/{month}/{day}");
                endpoints.MapRazorPages();
            });

        }
    }
}
