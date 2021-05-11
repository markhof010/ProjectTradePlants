using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantTrade.Models
{
    public partial class Report
    {
        public int Id { get; set; }
        public string Kind { get; set; }
        public string Description { get; set; }
        public string ReportId { get; set; }
        public string ReporterId { get; set; }

        public virtual User User { get; set; }

    }
}
