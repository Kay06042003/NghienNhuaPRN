using System;
using System.Collections.Generic;

#nullable disable

namespace Nghien_Nhua.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Text { get; set; }
        public DateTime Timetamp { get; set; }
        public int ConversationId { get; set; }
        public string IsFromShop { get; set; }
    }
}
