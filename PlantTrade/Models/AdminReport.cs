using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantTrade.Models
{
	public class AdminReport
	{
		public string Name { get; set; }
		public string Kind { get; set; }
		public int Count { get; set; }
		public Item Item { get; set; }
		public Tutorial Tutorial { get; set; }
		public User Profile { get; set; }
		public virtual IEnumerable<Report> Reports { get; set; }
	}
}
