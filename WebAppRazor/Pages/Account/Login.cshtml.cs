using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebAppRazor.Models;

namespace WebAppRazor.Pages.Account
{
    public class LoginModel : PageModel
    {

        private ILogger<LoginModel> logger;

        public LoginModel(ILogger<LoginModel> logger)
        {
            this.logger = logger;
        }

        private string ReturnUrl = null;

        public void OnGet()
        {
            ReturnUrl = HttpContext.Request.Query["ReturnUrl"];
            logger.LogInformation("Get��Login"+ " ReturnUrl="+ ReturnUrl);
        }

        /// <summary>
        /// ���ύ��post�е��ֶ���Ϣ
        /// </summary>
        [BindProperty]
        public LoginInput userInfo { get; set; }

        //����ֵ����ΪvoidҲ�����в���
        public IActionResult OnPost()
        {
            ReturnUrl = HttpContext.Request.Query["ReturnUrl"];
            logger.LogInformation("OnPost��Login");
            if (ModelState.IsValid)
            {
                logger.LogInformation($"{userInfo.UserName} {userInfo.Password}");
                if(userInfo.UserName=="111")
                    return RedirectToPage(ReturnUrl);
                else
                    return RedirectToPage("/Account/AccessDenied");
            }
            else
            {
                return RedirectToPage("/Account/AccessDenied");
            }
        }
    }

   
}
