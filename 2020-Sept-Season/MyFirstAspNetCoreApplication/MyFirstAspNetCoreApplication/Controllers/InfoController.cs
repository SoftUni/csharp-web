using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MyFirstAspNetCoreApplication.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFirstAspNetCoreApplication.Controllers
{
    public class InfoController : Controller
    {
        private readonly ILogger<InfoController> logger;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache cacheService;

        public InfoController(
            ILogger<InfoController> logger,
            IMemoryCache memoryCache,
            IDistributedCache cacheService)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
            this.cacheService = cacheService;
        }

        public IActionResult Time()
        {
            this.logger.LogWarning(12345, "User asked for the time.");

            if (!memoryCache.TryGetValue<DateTime>("Data", out var cacheTime))
            {
                cacheTime = GetData();
                memoryCache.Set("Data", cacheTime,
                    new MemoryCacheEntryOptions {
                        SlidingExpiration = new TimeSpan(0, 0, 2),
                    });
            }

            return this.Content(
                DateTime.Now.ToLongTimeString() + " -- Cache: " + cacheTime);
        }

        private DateTime GetData()
        {
            Thread.Sleep(5000);
            return DateTime.Now;
        }

        public async Task<IActionResult> Date()
        {
            this.logger.LogCritical(12345, "User asked for the date.");

            var dataAsString = await this.cacheService.GetStringAsync("DateTimeInfo");
            DateTime data;
            if (dataAsString == null)
            {
                data = this.GetData();
                await this.cacheService.SetStringAsync("DateTimeInfo",
                    JsonConvert.SerializeObject(data),
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = new TimeSpan(0, 0, 10)
                    });
            }
            else
            {
                data = JsonConvert.DeserializeObject<DateTime>(dataAsString);
            }

            return this.Content(data.ToString());
        }
    }
}
