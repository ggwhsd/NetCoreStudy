using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
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
        public async Task<IActionResult> OnPost()
        {
            //��ȡʱ�ĸ�ҳ���ύ����֤��¼����
            ReturnUrl = HttpContext.Request.Query["ReturnUrl"];
            if (string.IsNullOrEmpty(ReturnUrl) || Url.IsLocalUrl(ReturnUrl)==false)
                ReturnUrl = "/Index";
            logger.LogInformation("OnPost��Login");
            if (ModelState.IsValid) //�ύ��post����ͨ����LoginInput������Ч����֤������ֵ��userInfo����
            {
                logger.LogInformation($"{userInfo.UserName} {userInfo.Password}");
                if (ValidUserPassword(userInfo))  //������֤����֤ͨ����Ϊ�˺���������֤��������Ҫ����xxx������������ݵ�cookie��¼
                {
                    string role = "MyUser1Role";
                    if (userInfo.UserName == "111")
                        role = "MyUser1Role";
                    else if (userInfo.UserName == "222")
                        role = "MyUser2Role";
                    else if (userInfo.UserName == "000")
                        role = "Administrator";
                    else
                    {}

                    //����֤��������Ϣ
                    var claims = new List<Claim>
                    {
                        new Claim("username", userInfo.UserName),   //ClaimType= username 
                        new Claim(ClaimTypes.DateOfBirth, DateTime.Now.AddYears(-30).ToString(), null , "http://contoso.com"),
                        new Claim("role", role)                     //ClaimType= role 
                    };
                    //��֤���ԣ��趨��ʱΪ1����
                    var authProperties = new AuthenticationProperties();
                    authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1);

                    ///ClaimsPrincipal:�൱��֤��������Ϣ�����а����˻�����Ϣ-֤����;����-֤������Ϣ-֤���˽�ɫ
                    ///���ÿ�ܵĵ�¼���ܣ��Ǽ�֤�������ڿ�ܵ���Ȩ��֤���ԣ�ֻ���ClaimsPrincipal���Ӷ������������֤��Ȩ�����໥͸����
                    if (userInfo.UserName != "111")
                     { 
                    //��ӵ���֤��������
                    await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "cookies",
                        "username",    //��claims�е��ĸ�claimtype����ʾ�û�����
                        "role"   //��˼����˵ ��claims�е��ĸ�claimtype����ʾ��ɫ����
                        )), authProperties);
                    }
                    //111�ǳ����û�
                    # region ��Ӷ��֤��������
                    else if (userInfo.UserName == "111")
                    {
                            List<ClaimsIdentity> cis = new List<ClaimsIdentity>();
                            cis.Add(new ClaimsIdentity(claims, "Cookies",
                                "username",    //��claims�е��ĸ�claimtype����ʾ�û�����
                                "role"   //��˼����˵ ��claims�е��ĸ�claimtype����ʾ��ɫ����
                                ));
                            //����֤��������Ϣ
                        var claims2 = new List<Claim>
                        {
                            new Claim("username", userInfo.UserName),   //ClaimType= username 
                            new Claim("role", "Administrator")                     //ClaimType= role 
                        };
                        cis.Add(new ClaimsIdentity(claims2, "Cookies",
                          "username",    //��claims�е��ĸ�claimtype����ʾ�û�����
                          "role"   //��˼����˵ ��claims�е��ĸ�claimtype����ʾ��ɫ����
                          ));
                        await HttpContext.SignInAsync(new ClaimsPrincipal(cis), authProperties);
                        #endregion
                    }
                    return RedirectToPage(ReturnUrl);
                }
                else
                    return RedirectToPage("/Account/AccessDenied");
            }
            else
            {
                return RedirectToPage("/Account/AccessDenied");
            }
        }
        /// <summary>
        /// ��֤�û�������ȷ��,�˴�ʾ����ʵ���Ͽ���ȥ�������ݿ��������restful api������֤�ȵȡ�
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool ValidUserPassword(LoginInput userInput)
        {
            string[] users = {"111","222","000" };
            if (users.Contains(userInput.UserName) && userInput.Password == "password")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

   
}
