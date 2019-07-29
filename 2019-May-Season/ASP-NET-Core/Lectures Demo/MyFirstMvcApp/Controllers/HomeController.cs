using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyFirstMvcApp.Data;
using MyFirstMvcApp.Filters;
using MyFirstMvcApp.Models;
using MyFirstMvcApp.Models.Home;
using MyFirstMvcApp.Services;

namespace MyFirstMvcApp.Controllers
{
    public class IndexInputModel
    {
        [Required]
        [MinLength(5)]
        public string Name { get; set; }

        public int Age { get; set; }
    }

    [ServiceFilter(typeof(MyAuthorizeFilterAttribute))]
    public class HomeController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IConfiguration configuration;
        private readonly ICounterService counterService;
        private readonly ILogger<HomeController> logger;

        public HomeController(
            IUsersService usersService,
            IConfiguration configuration,
            ICounterService counterService,
            ILogger<HomeController> logger)
        {
            this.usersService = usersService;
            this.configuration = configuration;
            this.counterService = counterService;
            this.logger = logger;
        }

        public IActionResult Index(IndexInputModel input, int id)
        {
            if (id == 0)
            {
                this.logger.LogCritical(100, $"Id is {id}");
            }

            var viewModel = new IndexViewModel { Usernames = new[] { "niki" } };
            return View(viewModel);
        }

        [ValidateModelStateFilter]
        // Home/AcceptForm
        [HttpGet("niki/{id}")]
        public IActionResult AcceptForm(FormInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.Content("NOT OK!");
            }

            return this.Content("OK!");
        }

        public IActionResult Errors(string code)
        {
            return this.Content(code);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
