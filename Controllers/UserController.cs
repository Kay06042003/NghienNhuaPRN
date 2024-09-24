using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nghien_Nhua.Models;
using Nghien_Nhua.Middleware;

namespace Nghien_Nhua.Controllers
{
    [TypeFilter(typeof(UserAuthorizationFilter))]
    public class UserController : Controller
    {
        private readonly db_Nghien_NhuaContext _db;
        public UserController(db_Nghien_NhuaContext db)
        {
            _db = db;
        }
        public IActionResult UserInfo()
        {
            string email = HttpContext.Session.GetString("AccGmail");
            if (email == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Account acc = JsonConvert.DeserializeObject<Account>(email);
            User user = _db.Users.FirstOrDefault(x => x.AccId == acc.AccId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Account = acc;
            return View(user);
        }

        public IActionResult Edit()
        {
            string email = HttpContext.Session.GetString("AccGmail");
            if (email == null)
            {
                HttpContext.Session.SetString("status", "login");
                return RedirectToAction("Index", "Product");
            }
            Account acc = JsonConvert.DeserializeObject<Account>(email);
            User user1 = _db.Users.FirstOrDefault(x => x.AccId == acc.AccId);
            return View(user1);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            string email = HttpContext.Session.GetString("AccGmail");
            if (email == null)
            {
                HttpContext.Session.SetString("status", "login");
                return RedirectToAction("Index", "Product");
            }
            Account acc = JsonConvert.DeserializeObject<Account>(email);
            User user1 = _db.Users.FirstOrDefault(x => x.AccId == acc.AccId);
            user1.UserFullname = user.UserFullname;
            user1.UserSdt = user.UserSdt;
            user1.UserAddress = user.UserAddress;
            _db.Users.Update(user1);
            _db.SaveChanges();
            return RedirectToAction("UserInfo");
        }

    }
}