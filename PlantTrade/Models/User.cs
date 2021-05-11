using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PlantTrade.Models
{
    public partial class User : IdentityUser
    {
        public User()
        {
            ConversationTrader = new HashSet<Conversation>();
            ConversationUser = new HashSet<Conversation>();
            Item = new HashSet<Item>();
            Message = new HashSet<Message>();
            Tutorial = new HashSet<Tutorial>();
        }

        public string Postcode { get; set; }
        public string Specialisme { get; set; }
        public string About { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Conversation> ConversationTrader { get; set; }
        public virtual ICollection<Conversation> ConversationUser { get; set; }
        public virtual ICollection<Item> Item { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<Tutorial> Tutorial { get; set; }
        public virtual ICollection<Report> Report { get; set; }
    }
}
