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
        /// ���캯������ע�����ù�����ӿ�
        /// </summary>
        /// <param name="configRoot"></param>
        public ConfigShowModel(IConfiguration configRoot, IOptions<PositionOptions> options)
        {
            ConfigRoot = (IConfigurationRoot)configRoot;
            _options = options.Value;
        }

        //����ҳ��
        public void OnGet()
        {
           
            string str = "";
            //��ȡ����ʵ���б����� JsonConfigurationProvider
            //����json�����ļ������Ⱥ�˳��:appsettings.json,appsettings.{Environment}.json�� �����ͬ�������Ḳ��ǰ��ġ�
            //�ڳ��������ڼ䣬Ĭ��JsonConfigurationProvider�������ļ�ֻҪһ�޸ģ����������޸ĵġ����������ϼ���TestParamReloadOnFileChange������֮����һ������ҳ��Ϳ��Կ���ֵҲ���ˡ�
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

        //ֱ�ӷ�������
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
