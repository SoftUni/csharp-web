using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFirstMvcApp.Controllers
{
    public class CacheDemoController : Controller
    {
        private readonly IMemoryCache cache;
        private readonly IDistributedCache distributedCache;

        public CacheDemoController(IMemoryCache cache, IDistributedCache distributedCache)
        {
            this.cache = cache;
            this.distributedCache = distributedCache;
        }

        public IActionResult Test()
        {
            var stringValue = distributedCache.GetString("Now");
            if (stringValue == null)
            {
                stringValue = DateTime.UtcNow.ToString();
                distributedCache.SetString("Now", stringValue);
            }

            if (!cache.TryGetValue("Now", out DateTime value))
            {
                // Thread.Sleep(4000);
                value = DateTime.UtcNow;
                cache.Set("Now", value, new TimeSpan(0, 0, 5));
            }

            return this.View();
        }

        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Client)]
        public IActionResult ResponseCacheTest()
        {
            return Content(DateTime.UtcNow.ToString());
        }
    }
}
