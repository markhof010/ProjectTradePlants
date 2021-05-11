using System;
using System.Collections.Generic;

namespace PlantTrade.Models
{
    public partial class Tutorial
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string UserId { get; set; }
        public int Available { get; set; }

        public virtual User User { get; set; }
    }
}
