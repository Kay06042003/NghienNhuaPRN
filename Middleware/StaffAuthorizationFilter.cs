using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Nghien_Nhua.Middleware
{
    public class StaffAuthorizationFilter :  IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Session.GetString("role") == "1")
            {
                context.Result = new RedirectToActionResult("Index", "Product", null);
            }
        }
    }
}