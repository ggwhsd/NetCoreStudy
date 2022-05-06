using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace WebAppRazor.Pages
{
    public class ConfigShowModel : PageModel
    {
        private IConfigurationRoot ConfigRoot;

        public ConfigShowModel(IConfiguration configRoot)
        {
            ConfigRoot = (IConfigurationRoot)configRoot;
        }

        //返回页面
        public void OnGet()
        {
            string str = "";
            foreach (var provider in ConfigRoot.Providers.ToList())
            {
                str += provider.ToString() + "\r\n";
            }
            ViewData["Message"] = str;
            ViewData["Setting"] = ConfigRoot["DetailedErrors"] + " ~ "+ ConfigRoot["Logging:LogLevel:Default"];
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
