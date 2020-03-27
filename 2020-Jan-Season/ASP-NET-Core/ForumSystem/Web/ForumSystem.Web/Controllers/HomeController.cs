namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Services.Messaging;
    using ForumSystem.Web.ViewModels;
    using ForumSystem.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;

    public class HomeController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly ILogger<HomeController> logger;
        private readonly IDistributedCache distributedCache;

        public HomeController(
            ICategoriesService categoriesService,
            ILogger<HomeController> logger,
            IDistributedCache distributedCache)
        {
            this.categoriesService = categoriesService;
            this.logger = logger;
            this.distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            this.TempData["InfoMessage"] = "Thank you for visiting home page.";
            this.HttpContext.Session.SetString("Name", "Niki");
            this.logger.LogCritical("Hello!");
            var viewModel = new IndexViewModel
            {
                Categories =
                    this.categoriesService.GetAll<IndexCategoryViewModel>(),
            };
            return this.View(viewModel);
        }

        public IActionResult TempDataTest()
        {
            var value = this.TempData["InfoMessage"];
            if (value == null)
            {
                return this.Ok("empty temp data :(");
            }

            return this.Ok(value);
        }

        public IActionResult SessionTest()
        {
            var value = this.HttpContext.Session.GetString("Name");
            return this.Ok(value);
        }

        public async Task<IActionResult> CacheTest()
        {
            var data = await this.distributedCache.GetStringAsync("DateTimeAsString");
            if (data == null)
            {
                data = DateTime.UtcNow.ToString();
                await this.distributedCache.SetStringAsync("DateTimeAsString", data, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2),
                });
            }

            return this.Ok(data);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
