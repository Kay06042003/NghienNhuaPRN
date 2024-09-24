using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nghien_Nhua.Models;
using Nghien_Nhua.Services;
using Nghien_Nhua.Middleware;

namespace Nghien_Nhua.Controllers
{
    [TypeFilter(typeof(UserAuthorizationFilter))]
    public class PaymentController : Controller
    {
        private readonly IVnPayServices _vnPayServices;
        private readonly db_Nghien_NhuaContext _db;

        public PaymentController(IVnPayServices vnPayServices, db_Nghien_NhuaContext db)
        {
            _vnPayServices = vnPayServices;
            _db = db;
        }

        [Authorize]
        public IActionResult Return()
        {
            var response = _vnPayServices.PaymentExcute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                return RedirectToAction("Index", "Error");
            }
            string email = HttpContext.Session.GetString("AccGmail");
            Account account = JsonConvert.DeserializeObject<Account>(email);
            User user = _db.Users.FirstOrDefault(p => p.AccId == account.AccId);

            string orderJson = HttpContext.Session.GetString("Order");
            Order order = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(orderJson);
            _db.Orders.Add(order);
            _db.SaveChanges();

            IEnumerable<Cart> carts = _db.Carts.Where(p => p.UserId == user.UserId);
            IEnumerable<Product> AllProduct = _db.Products.ToList();
            List<Product> products = new List<Product>();
            Product pro = null;
            foreach (var item in carts)
            {
                pro = AllProduct.FirstOrDefault(p => p.ProId == item.ProId);
                products.Add(pro);
            }
            int orderID = _db.Orders.Where(p => p.UserId == user.UserId).OrderByDescending(p => p.OrderId).FirstOrDefault().OrderId;
            foreach (var item in carts)
            {
                OrderDetail oDetails = new OrderDetail();
                oDetails.OrderId = orderID;
                oDetails.ProId = item.ProId;
                oDetails.OdPrice = products.FirstOrDefault(p => p.ProId == item.ProId).ProPrice;
                oDetails.OdQuantity = item.CartQuantity;
                oDetails.OdTotalMoney = (item.CartQuantity * int.Parse(products.FirstOrDefault(p => p.ProId == item.ProId).ProPrice)).ToString();
                _db.OrderDetails.Add(oDetails);
            }
            _db.Carts.RemoveRange(carts);
            _db.SaveChanges();

            return View();
        }

    }
}