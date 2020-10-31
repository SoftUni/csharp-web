using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFirstAspNetCoreApplication.Data;
using MyFirstAspNetCoreApplication.Models;
using MyFirstAspNetCoreApplication.Service;
using MyFirstAspNetCoreApplication.ViewModels.Home;

namespace MyFirstAspNetCoreApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        public IActionResult Index(int id)
        {
            var viewModel = new IndexViewModel
            {
                Id = id,
                Name = "Anonymous",
                Year = DateTime.UtcNow.Year,
                Processors = Environment.ProcessorCount,
                UsersCount = this.dbContext.Users.Count(),
                Description = "Курсът \"ASP.NET Core\" ще ви научи как се изграждат съвременни уеб приложения с архитектурата Model-View-Controller, използвайки HTML5, бази данни, Entity Framework Core и други технологии. Изучава се технологичната платформа ASP.NET Core, нейните компоненти и архитектура, създаването на MVC уеб приложения, дефинирането на модели, изгледи и частични изгледи с Razor view engine, дефиниране на контролери, работа с бази данни и интеграция с Entity Framework Core, LINQ и SQL Server. Курсът включа и по-сложни теми като работа с потребители, роли и сесии, използване на AJAX, кеширане, сигурност на уеб приложенията, уеб сокети и работа с библиотеки от MVC контроли.",
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ContactForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ContactForm(string title, string content)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            // TODO: Save title and content in DB...

            return this.RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
