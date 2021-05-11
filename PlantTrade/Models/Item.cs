using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantTrade.Models
{
    public partial class Item
    {
        public Item()
        {
            Conversation = new HashSet<Conversation>();
        }

        public int Id { get; set; }
        public string Offer { get; set; }
        public string Kind { get; set; }
        public string Name { get; set; }
        public string LatinName { get; set; }
        public int SunHours { get; set; }
        public string Water { get; set; }
        public string Ground { get; set; }
        public string TransactionKind { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public int Available { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Conversation> Conversation { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
