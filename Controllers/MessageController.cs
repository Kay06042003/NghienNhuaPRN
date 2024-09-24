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

namespace Nghien_Nhua.Controllers
{
    public class MessageController : Controller
    {
        private readonly db_Nghien_NhuaContext _db;

        public MessageController(db_Nghien_NhuaContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // get user
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
            Message message = _db.Messages.FirstOrDefault(x => x.UserId == user.UserId);
            if (message == null)
            {
                message.ConversationId = _db.Messages.Max(x => x.ConversationId) + 1;
                message.UserId = user.UserId;
                message.Text = "";
                message.Timetamp = DateTime.Now;
                message.IsFromShop = "False";
            }
            return View(message);
        }

        [HttpPost]
        public IActionResult Index(Message message)
        {
            if (message.ConversationId == 0)
            {
                message.ConversationId = _db.Messages.Max(x => x.ConversationId) + 1;
            }
            message.Timetamp = DateTime.Now;
            message.IsFromShop = "False";
            UserSendMessage(message.Text, message.UserId??0, message.ConversationId);
            return View();
        }
        
        [HttpPost]
        public IActionResult UserSendMessage(string mess, int userID, int conversationID)
        {
            if (conversationID == 0)
            {
                conversationID = _db.Messages.Max(x => x.ConversationId) + 1;
            }
            Message message = new Message
            {
                ConversationId = conversationID,
                Text = mess,
                UserId = userID,
                Timetamp = DateTime.Now,
                IsFromShop = "False"
            };

            _db.Messages.Add(message);
            _db.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult AdminSendMessage(string mess, int conversationID, int userID)
        {
            Message message = new Message
            {
                ConversationId = conversationID,
                Text = mess,
                UserId = userID,
                Timetamp = DateTime.Now,
                IsFromShop = "True"
            };

            _db.Messages.Add(message);
            _db.SaveChanges();
            return Ok();
        }
    }
}
