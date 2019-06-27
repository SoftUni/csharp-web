using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMvcApp.Controllers
{
    public class OtherController : Controller
    {
        public IActionResult Index()
        {
            return this.Content("Other index");
        }
    }
}
