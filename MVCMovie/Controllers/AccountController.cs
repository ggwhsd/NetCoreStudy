using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVCMovie.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 此处returnUrl是启用了授权验证之后，如果登陆时发现没有验证，会记录当前请求的页面作为returnUrl，等验证成功后，返回该页面。
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        private bool ValidateLogin(string userName, string password)
        {
            // For this sample, all logins are successful.
            if (userName.Equals("OK"))
                return true;
            else
                return false;
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // Normally Identity handles sign in, but you can do it directly
            if (ValidateLogin(userName, password))
            {
                var claims = new List<Claim>
                {
                    new Claim("user", userName),
                    new Claim("role", "Member")
                };
                var authProperties = new AuthenticationProperties();
                authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(1);
                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")), authProperties);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return Redirect("/");
                }
            }
            else
                return View("AccessDenied");    
        }

        public IActionResult AccessDenied(string returnUrl = null)
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
