using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Options;
using WebAppRazor.Configuration;

namespace WebAppRazor.Pages
{
    [Authorize]
    public class ConfigShowModel : PageModel
    {
        private IConfigurationRoot ConfigRoot;

        private readonly PositionOptions _options;

        /// <summary>
        /// 构造函数依赖注入配置管理类接口
        /// </summary>
        /// <param name="configRoot"></param>
        public ConfigShowModel(IConfiguration configRoot, IOptions<PositionOptions> options)
        {
            ConfigRoot = (IConfigurationRoot)configRoot;
            _options = options.Value;
        }

        //返回页面
        public void OnGet()
        {
           
            string str = "";
            //获取配置实现列表，其中 JsonConfigurationProvider
            //对于json加载文件是有先后顺序:appsettings.json,appsettings.{Environment}.json。 后面的同名参数会覆盖前面的。
            //在程序运行期间，默认JsonConfigurationProvider是配置文件只要一修改，参数立即修改的。我在配置上加了TestParamReloadOnFileChange，改了之后，下一次请求页面就可以看到值也变了。
            foreach (var provider in ConfigRoot.Providers.ToList())
            {
                str += provider.ToString() + "\r\n";
            }
            ViewData["Message"] = str;
            
            ViewData["Setting"] = ConfigRoot["DetailedErrors"] + " ~ "+ ConfigRoot["Logging:LogLevel:Default"]+"~"+ConfigRoot["TestParamReloadOnFileChange"];

            
            var positionOptionsByBind = new PositionOptions();
            ConfigRoot.GetSection(PositionOptions.Position)
                      .Bind(positionOptionsByBind);
            ViewData["Options Pattern1"] = positionOptionsByBind.Name +"~" + positionOptionsByBind.Title;
           
            var positionOptionsByGet = ConfigRoot.GetSection(PositionOptions.Position)
                      .Get<PositionOptions>();
            ViewData["Options Pattern2"] = positionOptionsByGet.Name + "~" + positionOptionsByGet.Title;

            
            ViewData["Options Pattern3"] = _options.Name + "~" + _options.Title;

            ViewData["ASPNETCORE_ENVIRONMENT"] = ConfigRoot["ASPNETCORE_ENVIRONMENT"];
            ViewData["MyEnv1"] = ConfigRoot["MyEnv1"];
            ViewData["Path"] = ConfigRoot["Path"];
        }

        //直接返回内容
        //public ContentResult OnGet()
        //{
        //    string str = "";
        //    foreach (var provider in ConfigRoot.Providers.ToList())
        //    {
        //        str += provider.ToString() + "\n";
        //    }

        //    return Content(str);
        //}
    }
}
