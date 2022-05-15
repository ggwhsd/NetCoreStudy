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
            logger.LogInformation("Get了Login"+ " ReturnUrl="+ ReturnUrl);
        }

        /// <summary>
        /// 绑定提交的post中的字段信息
        /// </summary>
        [BindProperty]
        public LoginInput userInfo { get; set; }

        //返回值可以为void也可以有参数
        public async Task<IActionResult> OnPost()
        {
            //获取时哪个页面提交的认证登录请求
            ReturnUrl = HttpContext.Request.Query["ReturnUrl"];
            if (string.IsNullOrEmpty(ReturnUrl) || Url.IsLocalUrl(ReturnUrl)==false)
                ReturnUrl = "/Index";
            logger.LogInformation("OnPost了Login");
            if (ModelState.IsValid) //提交的post数据通过了LoginInput数据有效性验证，被赋值给userInfo对象。
            {
                logger.LogInformation($"{userInfo.UserName} {userInfo.Password}");
                if (ValidUserPassword(userInfo))  //密码验证，验证通过后，为了后续不再验证，所以需要调用xxx方法，进行身份的cookie记录
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

                    //创建证件基本信息
                    var claims = new List<Claim>
                    {
                        new Claim("username", userInfo.UserName),   //ClaimType= username 
                        new Claim(ClaimTypes.DateOfBirth, DateTime.Now.AddYears(-30).ToString(), null , "http://contoso.com"),
                        new Claim("role", role)                     //ClaimType= role 
                    };
                    //认证属性，设定超时为1分钟
                    var authProperties = new AuthenticationProperties();
                    authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1);

                    ///ClaimsPrincipal:相当于证件完整信息，其中包含了基本信息-证件用途类型-证件人信息-证件人角色
                    ///调用框架的登录功能，登记证件，对于框架的授权认证而言，只针对ClaimsPrincipal，从而做到与具体认证授权方法相互透明。
                    if (userInfo.UserName != "111")
                     { 
                    //添加单个证件持有人
                    await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "cookies",
                        "username",    //用claims中的哪个claimtype来表示用户名。
                        "role"   //意思就是说 用claims中的哪个claimtype来表示角色名。
                        )), authProperties);
                    }
                    //111是超级用户
                    # region 添加多个证件持有人
                    else if (userInfo.UserName == "111")
                    {
                            List<ClaimsIdentity> cis = new List<ClaimsIdentity>();
                            cis.Add(new ClaimsIdentity(claims, "Cookies",
                                "username",    //用claims中的哪个claimtype来表示用户名。
                                "role"   //意思就是说 用claims中的哪个claimtype来表示角色名。
                                ));
                            //创建证件基本信息
                        var claims2 = new List<Claim>
                        {
                            new Claim("username", userInfo.UserName),   //ClaimType= username 
                            new Claim("role", "Administrator")                     //ClaimType= role 
                        };
                        cis.Add(new ClaimsIdentity(claims2, "Cookies",
                          "username",    //用claims中的哪个claimtype来表示用户名。
                          "role"   //意思就是说 用claims中的哪个claimtype来表示角色名。
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
        /// 验证用户密码正确性,此处示例，实际上可以去访问数据库或者其他restful api进行验证等等。
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
