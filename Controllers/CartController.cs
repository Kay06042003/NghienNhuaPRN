using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Nghien_Nhua.Models;
using Nghien_Nhua.MyUtil;
using Nghien_Nhua.Middleware;

namespace Nghien_Nhua.Controllers
{
     [TypeFilter(typeof(UserAuthorizationFilter))]
    public class CartController : Controller
    {
        private readonly db_Nghien_NhuaContext _db;

        public CartController(db_Nghien_NhuaContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            String email = HttpContext.Session.GetString("AccGmail");
            if (email == null)
            {
                HttpContext.Session.SetString("status", "login");
                return RedirectToAction("Index", "Product");
            }
            Account account = JsonConvert.DeserializeObject<Account>(email);
            User user = _db.Users.FirstOrDefault(u => u.AccId == account.AccId);
            // get all product in cart
            IEnumerable<Cart> carts = _db.Carts.Where(c => c.UserId == user.UserId);
            if (carts.Count() == 0)
            {
                return RedirectToAction("CreateError", "Error", new { message = "Giỏ hàng của bạn đang trống" });
            }
            IEnumerable<Product> allProducts = _db.Products.ToList();
            // get product from carts
            List<Product> products = new List<Product>();
            Product pro = null;
            foreach (var item in carts)
            {
                pro = allProducts.FirstOrDefault(p => p.ProId == item.ProId);
                products.Add(pro);
            }
            // Send product to view
            ViewBag.products = products;
            return View(carts);
        }

        public IActionResult Delete(int id)
        {
            // get product from cart
            Cart cart = _db.Carts.FirstOrDefault(c => c.CartId == id);
            // remove product from cart
            _db.Carts.Remove(cart);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart(int cartId, int productId, int quantity)
        {
            // get product from cart
            Cart cart = _db.Carts.FirstOrDefault(c => c.CartId == cartId);
            // update quantity
            Product product = _db.Products.FirstOrDefault(p => p.ProId == productId);
            if (quantity > product.ProQuantity)
            {
                return Json(new { status = "error" });
            }
            cart.CartQuantity = quantity;
            _db.SaveChanges();
            int money = int.Parse(_db.Products.FirstOrDefault(p => p.ProId == productId).ProPrice);
            int quantityProductInCart = int.Parse(_db.Carts.FirstOrDefault(c => c.CartId == cartId).CartQuantity.ToString());
            int total = money * quantityProductInCart;
            int totalMoney = 0;
            IEnumerable<Cart> carts = _db.Carts.Where(c => c.UserId == cart.UserId);
            IEnumerable<Product> allProducts = _db.Products.ToList();

            foreach (var item in carts)
            {
                totalMoney += int.Parse(allProducts.FirstOrDefault(p => p.ProId == item.ProId).ProPrice) * int.Parse(item.CartQuantity.ToString());
            }
            int final = totalMoney + 20000;
            // convert money and totalMoeny before return
            Nghien_Nhua.MyUtil.ConvertFunction convert = new Nghien_Nhua.MyUtil.ConvertFunction();
            string totalMoneyString = convert.converterNumber(totalMoney);
            string totalString = convert.converterNumber(total);
            string finalString = convert.converterNumber(final);

            return Json(new { totalString, totalMoneyString, finalString });
        }

    }
}