using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlantTrade.Models;
using Microsoft.EntityFrameworkCore;


namespace PlantTrade.Controllers
{
	public class HomeController : Controller
	{
		private readonly planttradeContext _context;

		public HomeController(planttradeContext context)
		{
			_context = context;
		}


		public async Task<IActionResult> Index()
		{
			var size = await _context.Item.CountAsync();
			var planttradeContext = await _context.Item.Include(i => i.User).Where(m => m.Available == 1).ToListAsync();
			planttradeContext.Reverse();
			return View(planttradeContext.Take(4));
		}

		public IActionResult About()
		{
			return View();
		}

		public IActionResult Rules()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
