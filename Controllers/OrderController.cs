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
using Nghien_Nhua.Services;
using Nghien_Nhua.Middleware;

namespace Nghien_Nhua.Controllers
{
    [TypeFilter(typeof(UserAuthorizationFilter))]
    public class OrderController : Controller
    {
        private readonly db_Nghien_NhuaContext _db;
        private readonly IVnPayServices _vnPayServices;

        public OrderController(db_Nghien_NhuaContext db, IVnPayServices vnPayServices)
        {
            _db = db;
            _vnPayServices = vnPayServices;
        }


        public IActionResult Order()
        {
            string email = HttpContext.Session.GetString("AccGmail");
            if (email == null)
            {
                HttpContext.Session.SetString("status", "login");
                return RedirectToAction("Index", "Product");
            }
            Account account = JsonConvert.DeserializeObject<Account>(email);
            User user = _db.Users.FirstOrDefault(p => p.AccId == account.AccId);
            IEnumerable<Cart> carts = _db.Carts.Where(p => p.UserId == user.UserId);
            IEnumerable<Product> AllProduct = _db.Products.ToList();
            List<Product> products = new List<Product>();
            Product pro = null;
            foreach (var item in carts)
            {
                pro = AllProduct.FirstOrDefault(p => p.ProId == item.ProId);
                if (pro.ProQuantity < item.CartQuantity)
                {
                    HttpContext.Session.SetString("status", "ErrorOrder");
                    return RedirectToAction("Index", "Cart");
                }
                products.Add(pro);
            }
            ViewBag.products = products;
            return View(carts);
        }
        [HttpPost]
        public IActionResult Payment(IFormCollection form)
        {
            string name = form["txtName"];
            string phone = form["txtPhone"];
            string emailFrom = form["txtEmail"];
            string address = form["txtAddress"];
            string payment = form["txtpayment"];
            double total = Double.Parse(form["amount"]);
            string email = HttpContext.Session.GetString("AccGmail");
            Account account = JsonConvert.DeserializeObject<Account>(email);
            User user = _db.Users.FirstOrDefault(p => p.AccId == account.AccId);
            Order order = new Order();
            order.UserId = user.UserId;
            order.OrderDate = DateTime.Now;
            order.OrderStatus = "Waiting Accept - COD";
            order.OrderName = name;
            order.OrderPhoneNumber = phone;
            order.OrderEmail = emailFrom;
            order.OrderAddress = address;
            order.OrderTotalMoney = total.ToString();
            // convert order to json

            IEnumerable<Cart> carts = _db.Carts.Where(p => p.UserId == user.UserId);
            IEnumerable<Product> AllProduct = _db.Products.ToList();
            List<Product> products = new List<Product>();
            Product pro = null;
            foreach (var item in carts)
            {
                pro = AllProduct.FirstOrDefault(p => p.ProId == item.ProId);
                if (pro.ProQuantity < item.CartQuantity)
                {
                    item.CartQuantity = pro.ProQuantity;
                    _db.Carts.Update(item);
                    _db.SaveChanges();
                    HttpContext.Session.SetString("status", "ErrorOrder");
                    return RedirectToAction("Index", "Cart");
                }
                products.Add(pro);
            }

            switch (payment)
            {
                case "QuetMaQR":
                case "TKNganHang":
                case "QuocTe":
                    VnPayResqestModel vnPayment = new VnPayResqestModel
                    {
                        amount = total,
                        CreatedDate = DateTime.Now,
                        OrderId = 1,
                        fullName = name,
                        phone = phone,
                        email = emailFrom,
                        address = address
                    };
                    order.OrderStatus = "Accept - Banking";
                    string orderJson = Newtonsoft.Json.JsonConvert.SerializeObject(order);
                    HttpContext.Session.SetString("Order", orderJson);
                    return Redirect(_vnPayServices.CreateRequestUrl(HttpContext, vnPayment));
                case "NhanHang":
                    try
                    {
                        _db.Orders.Add(order);
                        _db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("Index", "Cart");
                    }

                    // get order id from order table with user id and last order date
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
                    break;
            }

            return RedirectToAction("Thanks", "Error");
        }

        // action history order
        public IActionResult History()
        {
            string email = HttpContext.Session.GetString("AccGmail");
            Account account = JsonConvert.DeserializeObject<Account>(email);
            User user = _db.Users.FirstOrDefault(p => p.AccId == account.AccId);
            IEnumerable<Order> orders = _db.Orders.Where(p => p.UserId == user.UserId).ToList();
            if (orders.Count() == 0)
            {
                return RedirectToAction("CreateError", "Error", new { message = "Bạn chưa có đơn đặt hàng nào trước đó!" });
            }
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in orders)
            {
                IEnumerable<OrderDetail> details = _db.OrderDetails.Where(p => p.OrderId == item.OrderId).ToList();
                foreach (var detail in details)
                {
                    orderDetails.Add(detail);
                }
            }
            List<Product> products = new List<Product>();
            foreach (var item in orderDetails)
            {
                Product pro = _db.Products.FirstOrDefault(p => p.ProId == item.ProId);
                products.Add(pro);
            }

            ViewBag.orderDetails = orderDetails;
            ViewBag.products = products;
            return View(orders);
        }
    }
}