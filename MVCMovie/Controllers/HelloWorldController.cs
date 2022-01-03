using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MVCMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        // GET: /HelloWorld/

        public string Index()
        {
            return "This is my default action...";
        }

        // 
        // GET: /HelloWorld/Welcome/ 

        public string Welcome()
        {
            return "This is the Welcome action method...";
        }

        // GET: /HelloWorld/Echo?name=ddd&numTimes=1 
        
        public string Echo(string name, int numTimes = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        }

        // GET: /HelloWorld/IDTest/3?name=ddd
        //因为在startup.cs中定义了模式pattern: "{controller=Home}/{action=Index}/{id?}");所以最后一个3自动归到参数id上
        public string IDTest(int id,string name)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, id is: {id}");
        }

        //返回welcomehtml.chtml
        public IActionResult WelcomeHtml(string name, int numTimes = 1)
        {
            //将数据传递给试图
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;
            return View();
        }
    }
}
