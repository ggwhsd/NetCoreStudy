using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAppRazor.Services.AuthorizonHandler
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        /// <summary>
        /// 处理特定的需求
        /// </summary>
        /// <param name="context">包含ClaimsPrincipal和resource</param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   MinimumAgeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth &&
                                            c.Issuer == "http://contoso.com"))
            {
                //TODO: Use the following if targeting a version of
                //.NET Framework older than 4.6:
                //      return Task.FromResult(0);
                return Task.CompletedTask;
            }

            var dateOfBirth = Convert.ToDateTime(
                context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth &&
                                            c.Issuer == "http://contoso.com").Value);
            int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))  { calculatedAge--; }

            if (calculatedAge >= requirement.MinimumAge) {  context.Succeed(requirement);  }

            return Task.CompletedTask;
        }
    }
}
