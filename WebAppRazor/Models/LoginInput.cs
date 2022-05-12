using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppRazor.Models
{
    public class LoginInput
    {
       
            [Required, StringLength(40, MinimumLength = 1)]
            public string UserName { get; set; }

            [Required, StringLength(20, MinimumLength = 1)]
            public string Password { get; set; }
       
    }
}
