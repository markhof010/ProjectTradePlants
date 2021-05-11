using System;
using System.Collections.Generic;

namespace PlantTrade.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public string Message1 { get; set; }
        public string TimeStamp { get; set; }
        public string UserId { get; set; }
        public int ConversationId { get; set; }

        public virtual Conversation Conversation { get; set; }
        public virtual User User { get; set; }
    }
}
