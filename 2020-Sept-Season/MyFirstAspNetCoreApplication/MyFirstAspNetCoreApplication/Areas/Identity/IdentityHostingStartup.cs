using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFirstAspNetCoreApplication.Data;
using MyFirstAspNetCoreApplication.Models;

[assembly: HostingStartup(typeof(MyFirstAspNetCoreApplication.Areas.Identity.IdentityHostingStartup))]
namespace MyFirstAspNetCoreApplication.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}