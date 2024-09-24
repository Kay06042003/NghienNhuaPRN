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
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class AdminController : Controller
    {
        private readonly db_Nghien_NhuaContext _db;
        private string acc;
        public AdminController(db_Nghien_NhuaContext db)
        {
            _db = db;
        }
        public IActionResult AddStaff()
        {
            acc = HttpContext.Session.GetString("AccGmail");
            if (acc == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Account account = JsonConvert.DeserializeObject<Account>(acc);
            if (account.Role != "3")
            {
                return RedirectToAction("Dashboard", "Staff");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddStaff(IFormCollection form)
        {
            try
            {
                string staff_email = form["txtStaff_Email"];
                string staff_pwd = form["txtStaff_Password"];
                string hash_pwd = Nghien_Nhua.Models.MD5.MD5Hash(staff_pwd);
                string staff_fullname = form["txtStaff_FullName"];
                string staff_gender = form["txtStaff_Gender"];
                string staff_phone = form["txtStaff_Phone"];
                string staff_address = form["txtStaff_Address"];
                string staff_birthday = form["txtStaff_Birthday"];
                string staff_citizenIdentityNumber = form["txtStaff_CitizenIdentityNumber"];
                string staff_salary = form["txtStaff_Salary"];
                Account account = new Account();
                account.AccPassword = hash_pwd;
                account.AccGmail = staff_email;
                account.Role = "2";
                _db.Accounts.Add(account);
                _db.SaveChanges();


                staff staffs = new staff();
                staffs.StaffFullname = staff_fullname;
                staffs.StaffGender = staff_gender;
                staffs.StaffPhoneNumber = staff_phone;
                staffs.StaffAddress = staff_address;
                staffs.StaffDateOfBirth = Convert.ToDateTime(staff_birthday);
                staffs.StaffCitizenIdentityNumber = staff_citizenIdentityNumber;
                staffs.StaffSalary = staff_salary;
                staffs.StaffDayJoin = DateTime.Now;
                staffs.AccId = account.AccId;
                staffs.StaffStatus = "Working";
                _db.staff.Add(staffs);
                _db.SaveChanges();
                HttpContext.Session.SetString("status", "success");
            }
            catch (Exception e)
            {
                HttpContext.Session.SetString("status", "error");
                return RedirectToAction("AddStaff", "Admin");
            }
            return RedirectToAction("Dashboard", "Staff");
        }

        public IActionResult ListStaff()
        {
            // TODO: Your code here
            acc = HttpContext.Session.GetString("AccGmail");
            if (acc == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Account account = JsonConvert.DeserializeObject<Account>(acc);
            if (account.Role != "3")
            {
                return RedirectToAction("Dashboard", "Staff");
            }
            IEnumerable<staff> staffs = _db.staff.ToList();
            return View(staffs);
        }

        public IActionResult EditStaff(int id)
        {
            acc = HttpContext.Session.GetString("AccGmail");
            if (acc == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Account account = JsonConvert.DeserializeObject<Account>(acc);
            if (account.Role != "3")
            {
                return RedirectToAction("Dashboard", "Staff");
            }

            staff staffs = _db.staff.Find(id);
            ViewBag.Account = account;
            return View(staffs);
        }

        [HttpPost]
        public IActionResult EditStaff(IFormCollection form)
        {
            try
            {
                string staff_email = form["txtStaff_Email"];
                string staff_fullname = form["txtStaff_FullName"];
                string staff_gender = form["txtStaff_Gender"];
                string staff_phone = form["txtStaff_Phone"];
                string staff_address = form["txtStaff_Address"];
                string staff_birthday = form["txtStaff_Birthday"];
                string staff_citizenIdentityNumber = form["txtStaff_CitizenIdentityNumber"];
                string staff_salary = form["txtStaff_Salary"];
                int staff_id = int.Parse(form["hidden_Id"]);
                string staffSalary = staff_salary.Replace(",", "");


                staff staffs = _db.staff.Find(staff_id);
                staffs.StaffId = staff_id;
                staffs.StaffFullname = staff_fullname;
                staffs.StaffGender = staff_gender;
                staffs.StaffPhoneNumber = staff_phone;
                staffs.StaffAddress = staff_address;
                staffs.StaffDateOfBirth = Convert.ToDateTime(staff_birthday);
                staffs.StaffCitizenIdentityNumber = staff_citizenIdentityNumber;
                staffs.StaffSalary = staffSalary;
                _db.staff.Update(staffs);
                _db.SaveChanges();
                HttpContext.Session.SetString("status", "success");
            }
            catch (Exception e)
            {
                HttpContext.Session.SetString("status", "error");
                return RedirectToAction("ListStaff", "Admin");
            }
            return RedirectToAction("ListStaff", "Admin");
        }

        public IActionResult Delete(int id)
        {
            acc = HttpContext.Session.GetString("AccGmail");
            if (acc == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Account account = JsonConvert.DeserializeObject<Account>(acc);
            if (account.Role != "3")
            {
                return RedirectToAction("Dashboard", "Staff");
            }
            try
            {
                staff staffs = _db.staff.Find(id);
                staffs.StaffStatus = "Resignation";
                _db.SaveChanges();
                HttpContext.Session.SetString("status", "success");
            }
            catch (Exception e)
            {
                HttpContext.Session.SetString("status", "error");
                return RedirectToAction("Dashboard", "Staff");
            }
            return RedirectToAction("ListStaff", "Admin");
        }

        public IActionResult Recover(int id)
        {
            acc = HttpContext.Session.GetString("AccGmail");
            if (acc == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Account account = JsonConvert.DeserializeObject<Account>(acc);
            if (account.Role != "3")
            {
                return RedirectToAction("Dashboard", "Staff");
            }
            try
            {
                staff staffs = _db.staff.Find(id);
                staffs.StaffStatus = "Working";
                _db.SaveChanges();
                HttpContext.Session.SetString("status", "success");
            }
            catch (Exception e)
            {
                HttpContext.Session.SetString("status", "error");
                return RedirectToAction("Dashboard", "Staff");
            }
            return RedirectToAction("ListStaff", "Admin");
        }





    }
}