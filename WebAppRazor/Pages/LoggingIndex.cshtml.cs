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
            _logger.LogInformation("��־����ʹ���ˣ���¼LoggingIndexModel");
            this.ViewData["Message"] = $"{DateTime.Now} ��־����ʹ���ˣ��ڿ���̨�Ͽ��Կ�����־��Ϣ��http����Host:{string.Join(",",this.HttpContext.Request.Host.Host)}  {this.HttpContext.Connection.RemoteIpAddress}-{this.HttpContext.Connection.RemotePort} {this.HttpContext.Connection.LocalIpAddress}";
            _logger.LogInformation(string.Join(",", this.Request.Headers.Keys));
        }
      
    }
}
