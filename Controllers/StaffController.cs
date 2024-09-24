using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nghien_Nhua.Models;
using Nghien_Nhua.Middleware;

namespace Nghien_Nhua.Controllers
{
    [TypeFilter(typeof(StaffAuthorizationFilter))]
    public class StaffController : Controller
    {
        private readonly db_Nghien_NhuaContext _db;

        public StaffController(db_Nghien_NhuaContext db)
        {
            _db = db;
        }

        public IActionResult Dashboard()
        {
            IEnumerable<Product> objList = _db.Products.ToList();
            return View(objList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormCollection form)
        {
            String uploadPath = "D:\\ProjectPRN\\Nghien_Nhua\\wwwroot\\Images\\Product";
            List<IFormFile> files = form.Files.ToList();
            string pic = "";
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = System.IO.Path.Combine(uploadPath, file.FileName);
                    using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    pic += file.FileName + "&";
                }
            }
            string name = form["txtName"];
            string price = form["txtPrice"];
            int quantity = int.Parse(form["txtQuantity"]);
            string des = form["txtDescription"];
            string discount = form["txtDiscount"];
            DateTime date = new DateTime();
            string category = form["txtCateName"];
            string brand = form["txtBrand"];
            string origin = form["txtOrigin"];
            Product product = new Product();
            product.ProName = name;
            product.ProPrice = price;
            product.ProQuantity = quantity;
            product.ProImage = pic;
            product.ProDescription = des;
            product.ProDiscount = discount;
            product.ProDate = date;
            product.ProCategory = category;
            product.ProBrand = brand;
            product.ProOrigin = origin;
            _db.Products.Add(product);
            _db.SaveChanges();
            switch (category)
            {
                case "Earphone":
                    Earphone earphone = new Earphone();
                    earphone.ProId = product.ProId;
                    earphone.EarType = form["type_earphone"];
                    earphone.EarPlug = form["plug_earphone"];
                    earphone.EarCompatibility = form["compatibility_earphone"];
                    earphone.EarWireLength = form["wirelength_earphone"];
                    earphone.EarUtility = form["utility_earphone"];
                    earphone.EarConnect = form["connect_earphone"];
                    earphone.EarControl = form["control_earphone"];
                    earphone.EarChargingPort = form["chargingport_earphone"];
                    earphone.EarConnectTech = form["connecttech_earphone"];
                    _db.Earphones.Add(earphone);
                    _db.SaveChanges();
                    break;
                case "Keycap":
                    Keycap keycap = new Keycap();
                    keycap.ProId = product.ProId;
                    keycap.KcMaterial = form["material_keycap"];
                    keycap.KcLayout = form["layout_keycap"];
                    keycap.KcThickness = form["thickness_keycap"];
                    keycap.KcReliability = form["reliability_keycap"];
                    _db.Keycaps.Add(keycap);
                    _db.SaveChanges();
                    break;
                case "Kit":
                    Kit kit = new Kit();
                    kit.ProId = product.ProId;
                    kit.KitLayout = form["layout_kit"];
                    kit.KitCircuit = form["circuit_kit"];
                    kit.KitPlate = form["plate_kit"];
                    kit.KitMode = form["mode_kit"];
                    kit.KitCase = form["case_kit"];
                    _db.Kits.Add(kit);
                    _db.SaveChanges();
                    break;
                case "Mouse":
                    Mouse mouse = new Mouse();
                    mouse.ProId = product.ProId;
                    mouse.MouseDpi = form["dpi_mouse"];
                    mouse.MouseWireLength = form["wirelength_mouse"];
                    mouse.MouseLed = form["led_mouse"];
                    mouse.MouseTypeBattery = form["typebattery_mouse"];
                    mouse.MouseWeight = form["weight_mouse"];
                    mouse.MouseCompatibility = form["compatibility_mouse"];
                    _db.Mice.Add(mouse);
                    _db.SaveChanges();
                    break;
                case "Switch":
                    Nghien_Nhua.Models.Switch switchs = new Nghien_Nhua.Models.Switch();
                    switchs.ProId = product.ProId;
                    switchs.SwitchPin = form["pin_switch"];
                    switchs.SwitchType = form["type_switch"];
                    switchs.SwitchSpring = form["spring_switch"];
                    switchs.SwitchReliability = form["reliability_switch"];
                    switchs.SwitchDepth = form["depth_switch"];
                    _db.Switches.Add(switchs);
                    _db.SaveChanges();
                    break;
            }
            return RedirectToAction("Dashboard");
        }

        public IActionResult Edit(int id)
        {
            Product product = _db.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }
            string category = product.ProCategory;
            switch (category)
            {
                case "Earphone":
                    Earphone earphone = _db.Earphones.FirstOrDefault(p => p.ProId == product.ProId);
                    if (earphone == null)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    ViewBag.Earphone = earphone;
                    break;
                case "Keycap":
                    Keycap keycap = _db.Keycaps.FirstOrDefault(p => p.ProId == product.ProId);
                    if (keycap == null)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    ViewBag.Keycap = keycap;
                    break;
                case "Kit":
                    Kit kit = _db.Kits.FirstOrDefault(p => p.ProId == product.ProId);
                    if (kit == null)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    ViewBag.Kit = kit;
                    break;
                case "Mouse":
                    Mouse mouse = _db.Mice.FirstOrDefault(p => p.ProId == product.ProId);
                    if (mouse == null)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    ViewBag.Mouse = mouse;
                    break;
                case "Switch":
                    Nghien_Nhua.Models.Switch switchs = _db.Switches.FirstOrDefault(p => p.ProId == product.ProId);
                    if (switchs == null)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    ViewBag.Switch = switchs;
                    break;
                case "Keyboard":
                    KeyBoard keyboard = _db.KeyBoards.FirstOrDefault(p => p.ProId == product.ProId);
                    if (keyboard == null)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    ViewBag.Keyboard = keyboard;
                    break;
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(IFormCollection form)
        {
            int id = int.Parse(form["hiddenID"]);
            string name = form["txtName"];
            string price = form["txtPrice"];
            int quantity = int.Parse(form["txtQuantity"]);
            string des = form["txtDescription"];
            string discount = form["txtDiscount"];
            DateTime date = new DateTime();
            string category = form["txtCateName"];
            Product product = _db.Products.Find(id);
            product.ProName = name;
            product.ProPrice = price;
            product.ProQuantity = quantity;
            product.ProDescription = des;
            product.ProDiscount = discount;
            product.ProDate = date;
            product.ProCategory = category;
            _db.Products.Update(product);
            _db.SaveChanges();
            switch (category)
            {
                case "Keyboard":
                    KeyBoard keyboard = _db.KeyBoards.FirstOrDefault(p => p.ProId == product.ProId);
                    if (keyboard == null)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    keyboard.ProId = product.ProId;
                    keyboard.KbLed = form["led_keyboard"];
                    keyboard.KbMode = form["mode_keyboard"];
                    keyboard.KbSwitch = form["switchType_keyboard"];
                    keyboard.KbKeycap = form["keycapType_keyboard"];
                    keyboard.KbPlate = form["plate_keyboard"];
                    keyboard.KbCase = form["case_keyboard"];
                    _db.KeyBoards.Update(keyboard);
                    _db.SaveChanges();
                    break;

                case "Earphone":
                    Earphone earphone = _db.Earphones.FirstOrDefault(p => p.ProId == product.ProId);
                    if (earphone == null)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    earphone.ProId = product.ProId;
                    earphone.EarType = form["type_earphone"];
                    earphone.EarPlug = form["plug_earphone"];
                    earphone.EarCompatibility = form["compatibility_earphone"];
                    earphone.EarWireLength = form["wirelength_earphone"];
                    earphone.EarUtility = form["utility_earphone"];
                    earphone.EarConnect = form["connect_earphone"];
                    earphone.EarControl = form["control_earphone"];
                    earphone.EarChargingPort = form["chargingport_earphone"];
                    earphone.EarConnectTech = form["connecttech_earphone"];
                    _db.Earphones.Update(earphone);
                    _db.SaveChanges();
                    break;

                case "Keycap":
                    Keycap keycap = _db.Keycaps.FirstOrDefault(p => p.ProId == product.ProId);
                    if (keycap == null)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    keycap.ProId = product.ProId;
                    keycap.KcMaterial = form["material_keycap"];
                    keycap.KcLayout = form["layout_keycap"];
                    keycap.KcThickness = form["thickness_keycap"];
                    keycap.KcReliability = form["reliability_keycap"];
                    _db.Keycaps.Update(keycap);
                    _db.SaveChanges();
                    break;

                case "Mouse":
                    Mouse mouse = _db.Mice.FirstOrDefault(p => p.ProId == product.ProId);
                    if (mouse == null)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    mouse.ProId = product.ProId;
                    mouse.MouseDpi = form["dpi_mouse"];
                    mouse.MouseWireLength = form["wirelength_mouse"];
                    mouse.MouseLed = form["led_mouse"];
                    mouse.MouseTypeBattery = form["typebattery_mouse"];
                    mouse.MouseWeight = form["weight_mouse"];
                    mouse.MouseCompatibility = form["compatibility_mouse"];
                    _db.Mice.Update(mouse);
                    _db.SaveChanges();
                    break;

                case "Kit":
                    Kit kit = _db.Kits.FirstOrDefault(p => p.ProId == product.ProId);
                    if (kit == null)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    kit.ProId = product.ProId;
                    kit.KitLayout = form["layout_kit"];
                    kit.KitCircuit = form["circuit_kit"];
                    kit.KitPlate = form["plate_kit"];
                    kit.KitMode = form["mode_kit"];
                    kit.KitCase = form["case_kit"];
                    _db.Kits.Update(kit);
                    _db.SaveChanges();
                    break;

                case "Switch":
                    Nghien_Nhua.Models.Switch switchs = _db.Switches.FirstOrDefault(p => p.ProId == product.ProId);
                    if (switchs == null)
                    {
                        return RedirectToAction("Dashboard");
                    }
                    switchs.ProId = product.ProId;
                    switchs.SwitchPin = form["pin_switch"];
                    switchs.SwitchType = form["type_switch"];
                    switchs.SwitchSpring = form["spring_switch"];
                    switchs.SwitchReliability = form["reliability_switch"];
                    switchs.SwitchDepth = form["depth_switch"];
                    _db.Switches.Update(switchs);
                    _db.SaveChanges();
                    break;

            }
            return RedirectToAction("Dashboard");
        }

        public IActionResult Delete(int id)
        {
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                HttpContext.Session.SetString("status", "error");
                return RedirectToAction("Dashboard");
            }
            product.ProQuantity = 0;
            _db.Products.Update(product);
            _db.SaveChanges();
            HttpContext.Session.SetString("status", "success");
            return RedirectToAction("Dashboard");
        }


        public IActionResult EditKeyboard(int id)
        {
            KeyBoard keyboard = _db.KeyBoards.Find(id);
            if (keyboard == null)
            {
                return RedirectToAction("Dashboard");
            }
            return View(keyboard);
        }
        public IActionResult EditMouse(int id)
        {
            Mouse mouse = _db.Mice.Find(id);
            if (mouse == null)
            {
                return RedirectToAction("Dashboard");
            }
            return View(mouse);
        }
        public IActionResult EditEarphone(int id)
        {
            Earphone earphone = _db.Earphones.Find(id);
            if (earphone == null)
            {
                return RedirectToAction("Dashboard");
            }
            return View(earphone);
        }
        public IActionResult EditKeycap(int id)
        {
            Keycap keycap = _db.Keycaps.Find(id);
            if (keycap == null)
            {
                return RedirectToAction("Dashboard");
            }
            return View(keycap);
        }
        public IActionResult EditKit(int id)
        {
            Kit kit = _db.Kits.Find(id);
            if (kit == null)
            {
                return RedirectToAction("Dashboard");
            }
            return View(kit);
        }
        public IActionResult EditSwitch(int id)
        {
            Nghien_Nhua.Models.Switch switchs = _db.Switches.Find(id);
            if (switchs == null)
            {
                return RedirectToAction("Dashboard");
            }
            return View(switchs);
        }

        public IActionResult ConfirmOrder()
        {
            string accJson = HttpContext.Session.GetString("AccGmail");
            if (accJson == null)
            {
                return RedirectToAction("Login", "Account");
            }
            IEnumerable<Order> orders = _db.Orders.Where(p => p.OrderStatus == "Waiting Accept - COD").ToList();
            if (orders.Count() == 0)
            {
                return RedirectToAction("ViewError", new { mess = "Không có đơn hàng nào cần xác nhận" });
            }
            IEnumerable<OrderDetail> orderDetails = _db.OrderDetails.ToList();
            List<OrderDetail> orderDetailsList = new List<OrderDetail>();
            foreach (var order in orders)
            {
                foreach (var orderDetail in orderDetails)
                {
                    if (orderDetail.OrderId == order.OrderId)
                    {
                        orderDetailsList.Add(orderDetail);
                    }
                }
            }
            List<Product> products = _db.Products.ToList();
            ViewBag.Products = products;
            ViewBag.OrderDetails = orderDetailsList;
            return View(orders);
        }

        [Route("Staff/Accept-Order/{id:int}")]
        public IActionResult AcceptOrder(int id)
        {
            Order order = _db.Orders.Find(id);
            if (order == null)
            {
                HttpContext.Session.SetString("status", "error");
                return RedirectToAction("ConfirmOrder");
            }
            List<OrderDetail> orderDetails = _db.OrderDetails.Where(p => p.OrderId == order.OrderId).ToList();

            foreach (var orderDetail in orderDetails)
            {
                Product product = _db.Products.Find(orderDetail.ProId);
                if (product == null)
                {
                    HttpContext.Session.SetString("status", "error");
                    return RedirectToAction("ConfirmOrder");
                }
                if (product.ProQuantity < orderDetail.OdQuantity)
                {
                    HttpContext.Session.SetString("status", "error");
                    return RedirectToAction("ConfirmOrder");
                }
                product.ProQuantity -= orderDetail.OdQuantity;
                _db.Products.Update(product);
            }
            order.OrderStatus = "Accepted - COD";
            _db.Orders.Update(order);
            _db.SaveChanges();
            HttpContext.Session.SetString("status", "success");
            return RedirectToAction("ConfirmOrder");
        }
        [Route("Staff/Reject-Order/{id:int}")]
        public IActionResult RejectOrder(int id)
        {
            Order order = _db.Orders.Find(id);
            if (order == null)
            {
                HttpContext.Session.SetString("status", "error");
                return RedirectToAction("ConfirmOrder");
            }
            order.OrderStatus = "Rejected - COD";
            _db.Orders.Update(order);
            _db.SaveChanges();
            HttpContext.Session.SetString("status", "success");
            return RedirectToAction("ConfirmOrder");
        }

        public IActionResult ViewError(string mess)
        {
            ViewData["Message"] = mess;
            return View();
        }

        public IActionResult StatisticsDay()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StatisticsDay(IFormCollection form)
        {
            string date = form["dates"];
            DateTime date1 = DateTime.Parse(date);
            // && p.OrderStatus == "Accepted"
            IEnumerable<Order> orders = _db.Orders.Where(p => p.OrderDate == date1 && p.OrderStatus.StartsWith("Accepted")).ToList();
            if (orders.Count() == 0)
            {
                return RedirectToAction("ViewError", new { mess = "Không có đơn hàng nào đã được bán trong ngày đó" });
            }
            IEnumerable<OrderDetail> orderDetails = _db.OrderDetails.ToList();
            List<OrderDetail> orderDetailsList = new List<OrderDetail>();
            foreach (var order in orders)
            {
                foreach (var orderDetail in orderDetails)
                {
                    if (orderDetail.OrderId == order.OrderId)
                    {
                        orderDetailsList.Add(orderDetail);
                    }
                }
            }
            List<Product> products = _db.Products.ToList();
            ViewBag.Products = products;
            ViewBag.OrderDetails = orderDetailsList;
            return View(orders);
        }

        public IActionResult StatisticsMonth()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StatisticsMonth(IFormCollection form)
        {
            string month = form["dates"];
            DateTime date1 = DateTime.Parse(month);

            IEnumerable<Order> orders = _db.Orders.Where(p => p.OrderDate.Month == date1.Month && p.OrderStatus.StartsWith("Accepted")).ToList();
            if (orders.Count() == 0)
            {
                return RedirectToAction("ViewError", new { mess = "Không có đơn hàng nào đã được bán trong tháng đó" });
            }
            IEnumerable<OrderDetail> orderDetails = _db.OrderDetails.ToList();
            List<OrderDetail> orderDetailsList = new List<OrderDetail>();
            foreach (var order in orders)
            {
                foreach (var orderDetail in orderDetails)
                {
                    if (orderDetail.OrderId == order.OrderId)
                    {
                        orderDetailsList.Add(orderDetail);
                    }
                }
            }
            List<Product> products = _db.Products.ToList();
            ViewBag.Products = products;
            ViewBag.OrderDetails = orderDetailsList;
            return View(orders);
        }

        public IActionResult StatisticsYear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StatisticsYear(IFormCollection form)
        {
            string year = form["dates"];
            DateTime date1 = DateTime.Parse(year);

            IEnumerable<Order> orders = _db.Orders.Where(p => p.OrderDate.Year == date1.Year && p.OrderStatus.StartsWith("Accepted")).ToList();
            if (orders.Count() == 0)
            {
                return RedirectToAction("ViewError", new { mess = "Không có đơn hàng nào đã được bán trong năm đó" });
            }
            IEnumerable<OrderDetail> orderDetails = _db.OrderDetails.ToList();
            List<OrderDetail> orderDetailsList = new List<OrderDetail>();
            foreach (var order in orders)
            {
                foreach (var orderDetail in orderDetails)
                {
                    if (orderDetail.OrderId == order.OrderId)
                    {
                        orderDetailsList.Add(orderDetail);
                    }
                }
            }
            List<Product> products = _db.Products.ToList();
            ViewBag.Products = products;
            ViewBag.OrderDetails = orderDetailsList;
            return View(orders);
        }

        public IActionResult Analysis()
        {
            // Đếm số lượng đơn hàng đã ở trạng thái Accepted
            IEnumerable<Order> orders = _db.Orders.Where(p => p.OrderStatus.StartsWith("Accepted")).ToList();
            IEnumerable<Order> orderDays = _db.Orders.Where(p => p.OrderStatus.StartsWith("Accepted") && p.OrderDate == DateTime.Now).ToList();
            int money = 0;
            foreach (var order in orders)
            {
                money += int.Parse(order.OrderTotalMoney);
            }

            int moneyDay = 0;
            foreach (var order in orderDays)
            {
                moneyDay += int.Parse(order.OrderTotalMoney);
            }
            ViewBag.money = money;
            ViewBag.moneyDay = moneyDay;
            // Đếm số lượng đơn hàng đang yêu cầu chờ xác nhận (Waiting Accept - COD)
            IEnumerable<Order> orders1 = _db.Orders.Where(p => p.OrderStatus == "Waiting Accept - COD").ToList();
            ViewBag.CountOrder1 = orders1.Count();

            List<View> view = _db.Views.ToList();
            List<View> viewDay = new List<View>();
            List<int> ints = new List<int>(Enumerable.Repeat(0, 31));
            List<int> int2 = new List<int>(Enumerable.Repeat(0, 31));

            for (var i = 0; i < view.Count(); i++)
            {
                string viewDate = DateTime.Now.ToString("yyyy-MM-dd");
                string getMonthOfYear = viewDate.Substring(0, 7);
                if (view[i].Date.StartsWith(getMonthOfYear))
                {
                    int day = int.Parse(view[i].Date.Substring(8, 2));
                    ints[day - 1] = view[i].Count.Value;
                }
            }

            for (int i = 0; i < orders.Count(); i++)
            {
                string yearNow = DateTime.Now.Year.ToString();
                string monthNow = DateTime.Now.Month.ToString();
                string year = orders.ElementAt(i).OrderDate.Year.ToString();
                string month = orders.ElementAt(i).OrderDate.Month.ToString();
                if (year.Equals(yearNow) && month.Equals(monthNow))
                {
                    int day = orders.ElementAt(i).OrderDate.Day;
                    int2[day - 1] += 1;
                }
            }
            var ints2Jon = Newtonsoft.Json.JsonConvert.SerializeObject(int2);
            var intsJon = Newtonsoft.Json.JsonConvert.SerializeObject(ints);
            ViewBag.ViewDay = intsJon;
            ViewBag.OrderDay = ints2Jon;

            return View();
        }


    }
}