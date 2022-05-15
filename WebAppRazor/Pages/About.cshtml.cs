using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppRazor.Pages
{

    [Authorize(Roles = "MyUser1Role")]
    [Authorize(Roles = "Administrator")]
    public class AboutModel : PageModel
    {
        //ÿ������scope��Χ�ڶ��ᴴ��һ���µ�AboutModelģ�ͣ�
        public String Message = DateTime.Now.ToLongTimeString();


      
        public void OnGet()
        {
            
            //Message = DateTime.Now.ToLongTimeString();
        }
    }
}