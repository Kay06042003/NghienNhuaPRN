using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Nghien_Nhua.Models;
using Nghien_Nhua.MyUtil;

namespace Nghien_Nhua.Controllers
{
    public class AccountController : Controller
    {
        private readonly db_Nghien_NhuaContext _db;
        private readonly ISendEmail _sendEmail;

        public AccountController(db_Nghien_NhuaContext db, ISendEmail sendEmail)
        {
            _db = db;
            _sendEmail = sendEmail;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Account acc)
        {
            acc.AccPassword = MD5.MD5Hash(acc.AccPassword);
            var account = _db.Accounts.FirstOrDefault(a => a.AccGmail == acc.AccGmail && a.AccPassword == acc.AccPassword);
            if (account != null)
            {
                string role = account.Role;
                string accountJon = JsonConvert.SerializeObject(account);
                // cookie to save success login and set time 3 days
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(3);
                option.Path = "/";
                Response.Cookies.Append("Account", accountJon, option);

                // session to save success login
                String accountJonn = JsonConvert.SerializeObject(account);
                HttpContext.Session.SetString("role", role);
                HttpContext.Session.SetString("Account", account.AccGmail);
                if (!role.Equals("1"))
                {
                    return RedirectToAction("Dashboard", "Staff");
                }
                return RedirectToAction("Index", "Product");
            }
            else
            {
                // sesssion to save fail login
                HttpContext.Session.SetString("status", "error");
                return View("Login");
            }
        }

        public IActionResult Logout()
        {
            // remove session
            HttpContext.Session.Remove("AccGmail");
            // remove cookie
            Response.Cookies.Delete("Account");
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(IFormCollection form)
        {
            var email = form["AccGmail"].ToString();
            var pw_hash = MD5.MD5Hash(form["AccPassword"].ToString());
            var fullName = form["UserFullname"].ToString();
            var phone = form["UserSdt"].ToString();
            var address = form["UserAddress"].ToString();

            // compare email
            var account = _db.Accounts.FirstOrDefault(a => a.AccGmail == email && a.AccPassword == pw_hash);
            if (account != null)
            {
                // sesstion mess error
                HttpContext.Session.SetString("status", "error");
                return View("Register");
            }
            else
            {
                Account acc = new Account();
                acc.AccGmail = email;
                acc.AccPassword = pw_hash;
                acc.Role = "1";
                User user = new User();
                user.UserFullname = fullName;
                user.UserSdt = phone;
                user.UserAddress = address;
                // send email
                int number = new Random().Next(100000, 999999);
                _sendEmail.SendEmailAsync(email, "Verify email", number, fullName);
                var userJon = JsonConvert.SerializeObject(user);
                var accJon = JsonConvert.SerializeObject(acc);
                // send user and account to next action
                TempData["code"] = number;
                TempData["acc"] = accJon;
                TempData["user"] = userJon;
            }
            return RedirectToAction("VerifyEmail");
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyEmail(IFormCollection form)
        {
            int code = int.Parse(form["code"].ToString());
            int number = int.Parse(TempData["code"].ToString());
            Account acc = JsonConvert.DeserializeObject<Account>(TempData["acc"].ToString());
            User user = JsonConvert.DeserializeObject<User>(TempData["user"].ToString());
            if (code == number)
            {
                _db.Accounts.Add(acc);
                _db.SaveChanges();
                user.AccId = acc.AccId;
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                HttpContext.Session.SetString("status", "error");
                return View();
            }
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(IFormCollection form)
        {
            var email = form["email"].ToString();
            var account = _db.Accounts.FirstOrDefault(a => a.AccGmail == email);
            if (account != null)
            {
                var user = _db.Users.FirstOrDefault(u => u.AccId == account.AccId);
                int number = new Random().Next(100000, 999999);
                _sendEmail.SendEmailAsync(email, "Verify email", number, user.UserFullname);
                TempData["code"] = number;
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                string acc = JsonConvert.SerializeObject(account, settings);
                HttpContext.Session.SetString("AccGmail", acc);
                return RedirectToAction("VerifyForgotPassword");
            }
            else
            {
                HttpContext.Session.SetString("status", "error");
                return View();
            }
        }

        public IActionResult VerifyForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyForgotPassword(IFormCollection form)
        {
            int code = int.Parse(form["code"].ToString());
            int number = int.Parse(TempData["code"].ToString());
            if (code == number)
            {
                return RedirectToAction("ChangePassword");
            }
            else
            {
                HttpContext.Session.SetString("status", "error");
                return View();
            }
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(IFormCollection form)
        {
            var password = MD5.MD5Hash(form["password"].ToString());
            var email = HttpContext.Session.GetString("AccGmail");
            var acc = JsonConvert.DeserializeObject<Account>(email);
            acc.AccPassword = password;
            _db.Accounts.Update(acc);
            _db.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult VerifyEmailChangePassword()
        {
            var acc = HttpContext.Session.GetString("AccGmail");
            Account account = JsonConvert.DeserializeObject<Account>(acc);
            User user = _db.Users.FirstOrDefault(u => u.AccId == account.AccId);
            int number = new Random().Next(100000, 999999);
            _sendEmail.SendEmailAsync(account.AccGmail, "Verify email", number, user.UserFullname);
            TempData["code"] = number;
            return View("VerifyForgotPassword");
        }

        [HttpPost]
        public IActionResult VerifyEmailChangePassword(IFormCollection form)
        {
            int code = int.Parse(form["code"].ToString());
            int number = int.Parse(TempData["code"].ToString());
            if (code == number)
            {
                return RedirectToAction("ChangePassword");
            }
            else
            {
                HttpContext.Session.SetString("status", "error");
                return View();
            }
        }

        public IActionResult CheckAccount()
        {
            string email = TempData["email"] as string;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }
            Account account = _db.Accounts.FirstOrDefault(a => a.AccGmail == email);
            if (account != null)
            {
                string role = account.Role;
                string accountJon = JsonConvert.SerializeObject(account);
                // cookie to save success login and set time 3 days
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(3);
                option.Path = "/";
                Response.Cookies.Append("Account", accountJon, option);

                // session to save success login
                String accountJonn = JsonConvert.SerializeObject(account);
                HttpContext.Session.SetString("role", role);
                HttpContext.Session.SetString("AccGmail", account.AccGmail);
                if (!role.Equals("1"))
                {
                    return RedirectToAction("Dashboard", "Staff");
                }
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ViewData["email"] = email;
                return View();
            }
        }

        [HttpPost]
        public IActionResult CheckAccount(IFormCollection form)
        {
            var email = form["txtEmail"].ToString();
            var fullName = form["txtName"].ToString();
            var phone = form["txtPhone"].ToString();
            var address = form["txtAddress"].ToString();
            var pw_hash = MD5.MD5Hash(form["txtPassword"].ToString());
            var account = _db.Accounts.FirstOrDefault(a => a.AccGmail == email && a.AccPassword == pw_hash);
            if (account != null)
            {
                string role = account.Role;
                string accountJon = JsonConvert.SerializeObject(account);
                // cookie to save success login and set time 3 days
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(3);
                option.Path = "/";
                Response.Cookies.Append("Account", accountJon, option);

                // session to save success login
                HttpContext.Session.SetString("role", role);
                HttpContext.Session.SetString("AccGmail", account.AccGmail);
                if (!role.Equals("1"))
                {
                    return RedirectToAction("Dashboard", "Staff");
                }
                return RedirectToAction("Index", "Product");
            }
            else
            {
                Account acc = new Account();
                acc.AccGmail = email;
                acc.AccPassword = pw_hash;
                acc.Role = "1";
                User user = new User();
                user.UserFullname = fullName;
                user.UserSdt = phone;
                user.UserAddress = address;
                _db.Accounts.Add(acc);
                _db.SaveChanges();
                Account account1 = _db.Accounts.FirstOrDefault(a => a.AccGmail == email);
                user.AccId = account1.AccId;
                _db.Users.Add(user);
                _db.SaveChanges();
                // cookie to save success login and set time 3 days
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                string accountJon = JsonConvert.SerializeObject(account1, settings);
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(3);
                option.Path = "/";
                Response.Cookies.Append("Account", accountJon, option);

                // session to save success login
                HttpContext.Session.SetString("role", acc.Role);
                HttpContext.Session.SetString("AccGmail", acc.AccGmail);
                return RedirectToAction("Index", "Product");
            }
        }



    }
}