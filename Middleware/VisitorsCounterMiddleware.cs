using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nghien_Nhua.Models;

namespace Nghien_Nhua.Middleware
{
    public class VisitorsCounterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly db_Nghien_NhuaContext _db;

        public VisitorsCounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var session = context.Session;

            // check if session is empty
            if (session.IsAvailable)
            {
                // check if session has a value
                if (session.GetString("visitors") == null)
                {
                    // set session value to 1
                    VisitorsDataAccess vs = new VisitorsDataAccess();
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    await vs.UpdateVisitorsCountInView(date, 1);
                    session.SetString("visitors", "visited");
                }
            }
            // call next middleware
            await _next(context);
        }

    }
}