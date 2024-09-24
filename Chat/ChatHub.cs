using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Nghien_Nhua.Models;

namespace Nghien_Nhua.Chat
{
    public class ChatHub : Hub
    {
        private readonly db_Nghien_NhuaContext _db;

        public ChatHub(db_Nghien_NhuaContext db)
        {
            _db = db;
        }

        public async Task SendMessage(int conversationID, string userID, string message, bool isFromShop)
        {
            if(conversationID == 0)
            {
                conversationID = _db.Messages.Max(x => x.ConversationId) + 1;
            }
            var msg = new Message
            {
                ConversationId = conversationID,
                Text = message,
                UserId = int.Parse(userID),
                Timetamp = DateTime.Now,
                IsFromShop = isFromShop ? "True" : "False"
            };

            _db.Messages.Add(msg);
            await _db.SaveChangesAsync();

            await Clients.Group(conversationID.ToString()).SendAsync("ReceiveMessage", userID, msg, isFromShop);
        }

        public async Task JoinConversation(int conversationID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationID.ToString());
        }

        public async Task LeaveConversation(int conversationID)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationID.ToString());
        }



    }
}