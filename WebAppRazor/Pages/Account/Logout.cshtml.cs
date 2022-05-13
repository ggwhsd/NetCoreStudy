using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppRazor.Pages.Account
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        public async void OnGet()
        {
            await HttpContext.SignOutAsync();
            
        }
    }
}
