using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAspNetCoreApplication.Areas.Administration.Controllers
{
    public class DashboardController : AdminController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
