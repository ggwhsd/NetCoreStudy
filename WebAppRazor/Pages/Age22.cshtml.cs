using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebAppRazor.Pages
{
    [Authorize(Policy= "AtLeast21",Roles ="MyUser1Role")]
    public class Age22Model : PageModel
    {
        public void OnGet()
        {
        }
    }
}
