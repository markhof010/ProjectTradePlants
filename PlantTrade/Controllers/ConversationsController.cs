using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantTrade.Models;

namespace PlantTrade.Controllers
{
    public class ConversationsController : Controller
    {
        private readonly planttradeContext _context;
        private readonly UserManager<User> _UserManager;

        public ConversationsController(planttradeContext context, UserManager<User> UserManager)
        {
            _context = context;
            this._UserManager = UserManager;
        }

        // GET: Conversations
        public async Task<IActionResult> Index()
        {
            var access = await _UserManager.GetUserAsync(User);
            if (access == null)
            {
                return NotFound();
            }
            var planttradeContext = _context.Conversation.Where(m => m.UserId == _UserManager.GetUserId(User) || m.TraderId == _UserManager.GetUserId(User)).Include(c => c.Item).Include(c => c.Trader).Include(c => c.User);
            return View(await planttradeContext.ToListAsync());
        }

        // GET: Conversations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var access = await _UserManager.GetUserAsync(User);
            if (id == null || access == null)
            {
                return NotFound();
            }

            var conversation = await _context.Conversation
                .Include(c => c.Item)
                .Include(c => c.Trader)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conversation == null)
            {
                return NotFound();
            }

            return View(conversation);
        }

        // GET: Conversations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var access = await _UserManager.GetUserAsync(User);

            if (id == null || access == null)
            {
                return NotFound();
            }

            var conversation = await _context.Conversation
                .Include(c => c.Item)
                .Include(c => c.Trader)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (conversation == null)
            {
                return NotFound();
            }

            return View(conversation);
        }

        // POST: Conversations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var access = await _UserManager.GetUserAsync(User);
            if (access == null)
            {
                return NotFound();
            }

            var conversation = await _context.Conversation.Where(m => m.Id == id).Include(m => m.Message).FirstOrDefaultAsync();
            foreach (Message message in conversation.Message)
			{
                _context.Message.Remove(message);
            }
            _context.Conversation.Remove(conversation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
