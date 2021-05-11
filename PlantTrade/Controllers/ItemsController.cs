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
    public class ItemsController : Controller
    {
        private readonly planttradeContext _context;

        [BindProperty]
        public List<bool> Zaadje { get; set; }

        public ItemsController(planttradeContext context)
        {
            _context = context;
        }

        // GET: Items
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var planttradeContext = await _context.Item.Where(m => m.Available == 1).Include(i => i.User).ToListAsync();
            planttradeContext.Reverse();
            return View(planttradeContext);
        }

        public async Task<IActionResult> ViewItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.User)
                .Include(i => i.Conversation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        public async Task<IActionResult> UserInformation(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.Where(x => x.Id == id).Select(x => new User { Id = x.Id ,UserName = x.UserName, Specialisme = x.Specialisme, About = x.About }).ToListAsync();

            if (user == null || user.Count() < 1 || user.Count() > 1)
            {
                return NotFound();
            }

            ViewBag.userId = id;
            return View(user[0]);
        }

        public async Task<IActionResult> UserItems(string id)
        {
            var Items = await _context.Item.Where(m => m.UserId == id && m.Available == 1).ToListAsync();
            if (Items == null)
            {
                return NotFound();
            }
            Items.Reverse();
            ViewBag.userId = id;
            return View(Items);
        }

        public async Task<IActionResult> UserTutorials(string id)
        {
            var tutorials = await _context.Tutorial.Include(m => m.User).Where(m => m.UserId == id).ToListAsync();
            if (tutorials == null)
            {
                return NotFound();
            }
            tutorials.Reverse();
            ViewBag.userId = id;
            return View(tutorials);
        }
    }
}
