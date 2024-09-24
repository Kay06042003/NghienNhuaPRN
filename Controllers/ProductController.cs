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

    public class ProductController : Controller
    {
        private readonly db_Nghien_NhuaContext _db;
        private Account account = null;
        public ProductController(db_Nghien_NhuaContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            // select top 8 * from Product order by pro_id desc
            IEnumerable<Product> product = _db.Products.OrderByDescending(p => p.ProId).Take(8).ToList();
            // sesstion to save product
            return View(product);
        }

        public IActionResult Keyboard()
        {
            IEnumerable<Product> product = _db.Products.OrderByDescending(p => p.ProId).Take(2).ToList();
            String productJson = Newtonsoft.Json.JsonConvert.SerializeObject(product);
            HttpContext.Session.SetString("newProduct", productJson);
            IEnumerable<Product> products = _db.Products.Where(p => p.ProCategory == "Keyboard").ToList();
            return View(products);
        }

        public IActionResult Mouse()
        {
            IEnumerable<Product> product = _db.Products.OrderByDescending(p => p.ProId).Take(2).ToList();
            String productJson = Newtonsoft.Json.JsonConvert.SerializeObject(product);
            HttpContext.Session.SetString("newProduct", productJson);
            IEnumerable<Product> products = _db.Products.Where(p => p.ProCategory == "Mouse").ToList();
            return View("Keyboard", products);
        }

        public IActionResult Earphone()
        {
            IEnumerable<Product> product = _db.Products.OrderByDescending(p => p.ProId).Take(2).ToList();
            String productJson = Newtonsoft.Json.JsonConvert.SerializeObject(product);
            HttpContext.Session.SetString("newProduct", productJson);
            IEnumerable<Product> products = _db.Products.Where(p => p.ProCategory == "Earphone").ToList();
            return View("Keyboard", products);
        }

        public IActionResult Switch()
        {
            IEnumerable<Product> product = _db.Products.OrderByDescending(p => p.ProId).Take(2).ToList();
            String productJson = Newtonsoft.Json.JsonConvert.SerializeObject(product);
            HttpContext.Session.SetString("newProduct", productJson);
            IEnumerable<Product> products = _db.Products.Where(p => p.ProCategory == "Switch").ToList();
            return View("Keyboard", products);
        }

        public IActionResult Keycap()
        {
            IEnumerable<Product> product = _db.Products.OrderByDescending(p => p.ProId).Take(2).ToList();
            String productJson = Newtonsoft.Json.JsonConvert.SerializeObject(product);
            HttpContext.Session.SetString("newProduct", productJson);
            IEnumerable<Product> products = _db.Products.Where(p => p.ProCategory == "Keycap").ToList();
            return View("Keyboard", products);
        }

        public IActionResult Kit()
        {
            IEnumerable<Product> product = _db.Products.OrderByDescending(p => p.ProId).Take(2).ToList();
            String productJson = Newtonsoft.Json.JsonConvert.SerializeObject(product);
            HttpContext.Session.SetString("newProduct", productJson);
            IEnumerable<Product> products = _db.Products.Where(p => p.ProCategory == "Kit").ToList();
            return View("Keyboard", products);
        }

        public IActionResult ProductDetail(int id, string proCategory)
        {
            IEnumerable<Product> productSimilar = _db.Products.Where(p => p.ProCategory == proCategory).Take(2).ToList();
            String productSimilarJon = Newtonsoft.Json.JsonConvert.SerializeObject(productSimilar);
            HttpContext.Session.SetString("productSimilar", productSimilarJon);

            if (proCategory.Equals("Keyboard"))
            {
                var product = _db.Products.FirstOrDefault(p => p.ProId == id && p.ProCategory == "Keyboard");
                var keyboard = _db.KeyBoards.FirstOrDefault(k => k.ProId == product.ProId);
                ProductKeyboard productKeyboard = new ProductKeyboard();
                productKeyboard.Product = product;
                productKeyboard.Keyboard = keyboard;

                return View("ProductDetail", productKeyboard);
            }
            else if (proCategory.Equals("Mouse"))
            {
                var product = _db.Products.FirstOrDefault(p => p.ProId == id && p.ProCategory == "Mouse");
                var mouse = _db.Mice.FirstOrDefault(m => m.ProId == product.ProId);
                ProductKeyboard productKeyboard = new ProductKeyboard();
                productKeyboard.Product = product;
                productKeyboard.mouse = mouse;

                return View("ProductDetail", productKeyboard);
            }
            else if (proCategory.Equals("Earphone"))
            {
                var product = _db.Products.FirstOrDefault(p => p.ProId == id && p.ProCategory == "Earphone");
                var earphone = _db.Earphones.FirstOrDefault(e => e.ProId == product.ProId);
                ProductKeyboard productKeyboard = new ProductKeyboard();
                productKeyboard.Product = product;
                productKeyboard.earphone = earphone;

                return View("ProductDetail", productKeyboard);
            }
            else if (proCategory.Equals("Switch"))
            {
                var product = _db.Products.FirstOrDefault(p => p.ProId == id && p.ProCategory == "Switch");
                var switchs = _db.Switches.FirstOrDefault(s => s.ProId == product.ProId);
                ProductKeyboard productKeyboard = new ProductKeyboard();
                productKeyboard.Product = product;
                productKeyboard.switchs = switchs;

                return View("ProductDetail", productKeyboard);
            }
            else if (proCategory.Equals("Keycap"))
            {
                var product = _db.Products.FirstOrDefault(p => p.ProId == id && p.ProCategory == "Keycap");
                var keycap = _db.Keycaps.FirstOrDefault(k => k.ProId == product.ProId);
                ProductKeyboard productKeyboard = new ProductKeyboard();
                productKeyboard.Product = product;
                productKeyboard.keycap = keycap;


                return View("ProductDetail", productKeyboard);
            }
            else if (proCategory.Equals("Kit"))
            {
                var product = _db.Products.FirstOrDefault(p => p.ProId == id && p.ProCategory == "Kit");
                var kit = _db.Kits.FirstOrDefault(k => k.ProId == product.ProId);
                ProductKeyboard productKeyboard = new ProductKeyboard();
                productKeyboard.Product = product;
                productKeyboard.kit = kit;

                return View("ProductDetail", productKeyboard);
            }
            return View("Product", "Index");
        }

        [ServiceFilter(typeof(UserAuthorizationFilter))]
        public IActionResult AddToCart(ProductKeyboard pro)
        {
            // check login
            String accJon = HttpContext.Session.GetString("AccGmail");

            if (accJon == null || accJon.Equals(""))
            {
                HttpContext.Session.SetString("status", "login");
                return RedirectToAction("ProductDetail", "Product", new { id = pro.Product.ProId, proCategory = pro.Product.ProCategory });
            }
            int quantity = (int)pro.Product.ProQuantity;
            int proId = pro.Product.ProId;
            int quantityProduct = int.Parse(_db.Products.FirstOrDefault(p => p.ProId == proId).ProQuantity.ToString());
            if (quantity > quantityProduct)
            {
                HttpContext.Session.SetString("status", "ErrorAddtoCart");
                return RedirectToAction("ProductDetail", "Product", new { id = pro.Product.ProId, proCategory = pro.Product.ProCategory });
            }
            else
            {
                Account account = JsonConvert.DeserializeObject<Account>(accJon);
                User user = _db.Users.FirstOrDefault(u => u.AccId == account.AccId);
                // check product in cart
                Cart cart = _db.Carts.FirstOrDefault(c => c.ProId == pro.Product.ProId && c.UserId == user.UserId);
                if (cart != null)
                {
                    cart.CartQuantity += quantity;
                    _db.Carts.Update(cart);
                    _db.SaveChanges();
                }
                else
                {
                    // get user id from database

                    Cart newCart = new Cart();
                    newCart.ProId = pro.Product.ProId;
                    newCart.CartQuantity = pro.Product.ProQuantity;
                    newCart.UserId = user.UserId;
                    _db.Carts.Add(newCart);
                    _db.SaveChanges();
                    HttpContext.Session.SetString("status", "success");
                    return RedirectToAction("ProductDetail", "Product", new { id = pro.Product.ProId, proCategory = pro.Product.ProCategory });
                }
            }
            HttpContext.Session.SetString("status", "success");
            return RedirectToAction("ProductDetail", "Product", new { id = pro.Product.ProId, proCategory = pro.Product.ProCategory });
        }

        public IActionResult Search(string txt)
        {
            IEnumerable<Product> products = _db.Products.Where(p => p.ProName.Contains(txt)).ToList();
            return Json(products);
        }
    }
}