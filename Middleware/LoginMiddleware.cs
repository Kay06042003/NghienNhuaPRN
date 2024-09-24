using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nghien_Nhua.Models;


namespace Nghien_Nhua.Middleware
{
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Cookies["Account"] != null)
            {
                // get data in cookie
                string account = context.Request.Cookies["Account"];
                string role = Newtonsoft.Json.JsonConvert.DeserializeObject<Account>(account).Role;
                context.Session.SetString("role", role);
                context.Session.SetString("AccGmail", context.Request.Cookies["Account"]);
            }
            await _next(context);
        }
    }
}