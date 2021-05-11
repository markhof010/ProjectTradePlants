using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantTrade.Models;

namespace PlantTrade.Controllers
{
    public class TutorialsController : Controller
    {
        private readonly planttradeContext _context;

        public TutorialsController(planttradeContext context)
        {
            _context = context;
        }

        // GET: Tutorials
        public async Task<IActionResult> Index()
        {
            var planttradeContext = await _context.Tutorial.Include(t => t.User).ToListAsync();

            planttradeContext.Reverse();

            return View(planttradeContext);
        }

    }
}
