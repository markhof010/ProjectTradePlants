using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantTrade.Models
{
	public class Feedback
	{
        public int Id { get; set; }
        public string Page { get; set; }
        public string FeedbackText { get; set; }
        public string UserId { get; set; }
    }
}
