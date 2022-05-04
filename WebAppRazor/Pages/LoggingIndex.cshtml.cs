using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebAppRazor.Pages
{
    public class LoggingIndexModel : PageModel
    {

        private readonly ILogger<LoggingIndexModel> _logger;

        public LoggingIndexModel(ILogger<LoggingIndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("日志可以使用了，记录LoggingIndexModel");
            this.ViewData["Message"] = $"{DateTime.Now} 日志可以使用了，在控制台上可以看到日志信息。http请求Host:{string.Join(",",this.HttpContext.Request.Host.Host)}  {this.HttpContext.Connection.RemoteIpAddress}-{this.HttpContext.Connection.RemotePort} {this.HttpContext.Connection.LocalIpAddress}";
            _logger.LogInformation(string.Join(",", this.Request.Headers.Keys));
        }
      
    }
}
