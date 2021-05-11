using System;
using System.Collections.Generic;

namespace PlantTrade.Models
{
    public partial class Conversation
    {
        public Conversation()
        {
            Message = new HashSet<Message>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string TraderId { get; set; }
        public int ItemId { get; set; }

        public virtual Item Item { get; set; }
        public virtual User Trader { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Message> Message { get; set; }
    }
}
