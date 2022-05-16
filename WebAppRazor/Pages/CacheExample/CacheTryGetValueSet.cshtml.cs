using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using WebAppRazor.Models;

namespace WebAppRazor.Pages.CacheExample
{
    public class CacheTryGetValueSetModel : PageModel
    {
        private IMemoryCache _cache;

        public CacheTryGetValueSetModel(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }
        public void OnGet()
        {
            DateTime cacheEntry;

            // Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Entry, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = DateTime.Now;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(5));

                // Save data in cache.
                _cache.Set(CacheKeys.Entry, cacheEntry, cacheEntryOptions);
            }
            ViewData["Message"] = "以下数据为缓存数据，因为设置了滑动超时时间3秒和绝对超时时间5秒，如果超过3秒没有人访问过该缓存，或者即使有人访问但是超过了5秒，缓存数据就会变化。";
            ViewData["Cache"] = cacheEntry;
        }
    }
}
