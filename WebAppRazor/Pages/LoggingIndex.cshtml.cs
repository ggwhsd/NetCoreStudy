using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebAppRazor.Services;

namespace WebAppRazor.Pages
{
    public class LoggingIndexModel : PageModel
    {

        private readonly ILogger<LoggingIndexModel> _logger;
        private ILog _log4;

        public LoggingIndexModel(ILogger<LoggingIndexModel> logger, MyLog4NetService log4, TemplateServiceInterface<int> myInter)
        {
            _logger = logger;
            _log4 = log4.GetLogger<LoggingIndexModel>();
            _logger.LogInformation("自定义的泛型接口服务依赖注入的调用:"+myInter.Serve(3));
        }

        public void OnGet()
        {
            _logger.LogInformation("日志可以使用了，记录LoggingIndexModel");
            this.ViewData["Message"] = $"{DateTime.Now} 日志可以使用了，在控制台上可以看到日志信息。http请求Host:{string.Join(",",this.HttpContext.Request.Host.Host)}  {this.HttpContext.Connection.RemoteIpAddress}-{this.HttpContext.Connection.RemotePort} {this.HttpContext.Connection.LocalIpAddress}";
            _logger.LogInformation(string.Join(",", this.Request.Headers.Keys));

            _log4.Info("这一行是log4Net的日志模块输出");
        }
      
    }
}
