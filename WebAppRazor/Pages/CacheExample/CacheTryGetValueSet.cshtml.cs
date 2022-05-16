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
            ViewData["Message"] = "��������Ϊ�������ݣ���Ϊ�����˻�����ʱʱ��3��;��Գ�ʱʱ��5�룬�������3��û���˷��ʹ��û��棬���߼�ʹ���˷��ʵ��ǳ�����5�룬�������ݾͻ�仯��";
            ViewData["Cache"] = cacheEntry;
        }
    }
}
