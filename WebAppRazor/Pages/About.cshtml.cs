using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppRazor.Pages
{
    public class AboutModel : PageModel
    {
        //每个请求scope范围内都会创建一个新的AboutModel模型，
        public String Message = DateTime.Now.ToLongTimeString();



        public void OnGet()
        {
            
            //Message = DateTime.Now.ToLongTimeString();
        }
    }
}
