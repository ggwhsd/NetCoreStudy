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
            _logger.LogInformation("�Զ���ķ��ͽӿڷ�������ע��ĵ���:"+myInter.Serve(3));
        }

        public void OnGet()
        {
            _logger.LogInformation("��־����ʹ���ˣ���¼LoggingIndexModel");
            this.ViewData["Message"] = $"{DateTime.Now} ��־����ʹ���ˣ��ڿ���̨�Ͽ��Կ�����־��Ϣ��http����Host:{string.Join(",",this.HttpContext.Request.Host.Host)}  {this.HttpContext.Connection.RemoteIpAddress}-{this.HttpContext.Connection.RemotePort} {this.HttpContext.Connection.LocalIpAddress}";
            _logger.LogInformation(string.Join(",", this.Request.Headers.Keys));

            _log4.Info("��һ����log4Net����־ģ�����");
        }
      
    }
}
