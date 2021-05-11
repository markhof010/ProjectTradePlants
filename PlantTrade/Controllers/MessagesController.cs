using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlantTrade.Models;

namespace PlantTrade.Controllers
{
    public class MessagesController : Controller
    {
        private readonly planttradeContext _context;
        private readonly UserManager<User> _UserManager;

        public MessagesController(planttradeContext context, UserManager<User> userManager)
        {
            _context = context;
            _UserManager = userManager;
        }

        // GET: Messages
        public async Task<IActionResult> Index(int id)
        {
            var access = await _UserManager.GetUserAsync(User);
            if (access == null)
            {
                return NotFound();
            }
            var planttradeContext = _context.Message.Where(m => m.ConversationId == id);

            return View(await planttradeContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateConversation(int Id)
        {
            Conversation conversation = new Conversation();
            Item item = _context.Item.Where(item => item.Id == Id).FirstOrDefault();
            string userId = _UserManager.GetUserId(User);

            conversation.User = _context.User.Where(user => user.Id == userId).First();
            conversation.UserId = userId;
            conversation.Trader = _context.User.Where(user => user.Id == item.UserId).First();
            conversation.TraderId = item.UserId;
            conversation.Item = item;
            conversation.ItemId = item.Id;

            _context.Add(conversation);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewItem", "Items", new { id = item.Id });
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] Message message)
        {
            if (ModelState.IsValid)
            {
                message.TimeStamp = DateTime.Now.ToString();

                _context.Add(message);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException error)
                {
                    var test = error;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(message);
        }

        [HttpGet]
        public string GetConversationWithItem(int id)
        {
            string curUserId = _UserManager.GetUserId(User);
            Conversation conv = _context.Conversation.Where(conv => conv.ItemId == id && conv.UserId == curUserId).FirstOrDefault();

            if (conv != null)
            {
                return GetConversation(conv.Id);
            }

            return JsonConvert.SerializeObject(new Dictionary<string, string>() { { "TraderName", "INVALID" } });
        }

        [HttpGet]
        public string GetConversation(int id)
        {
            string curUserId = _UserManager.GetUserId(User);

            try
            {
                if (_context.Conversation.Where(c => c.Id == id).FirstOrDefault() != null)
                {
                    if (_context.Conversation.Where(c => c.Id == id).FirstOrDefault().UserId == curUserId
                        || _context.Conversation.Where(c => c.Id == id).FirstOrDefault().TraderId == curUserId)
                    {
                        return JsonConvert.SerializeObject(_context.Conversation.Where(c => c.Id == id)
                            .Join(_context.User, conv => conv.UserId, user => user.Id, (conv, user) => new Dictionary<string, string> {
                                { "TraderName", _context.User.Where(user => user.Id == (curUserId == conv.UserId ? conv.TraderId : conv.UserId)).FirstOrDefault().UserName },
                                { "UserId", curUserId },
                                { "ConversationId", conv.Id.ToString() },
                                { "SenderId", curUserId == conv.UserId ? conv.UserId : conv.TraderId },
                                { "TraderId", curUserId == conv.UserId ? conv.TraderId : conv.UserId }
                            }).FirstOrDefault());
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new Dictionary<string, string>() { { "TraderName", "INVALID" } });
                    }
                }

                return JsonConvert.SerializeObject(new Dictionary<string, string>() { { "TraderName", "INVALID" } });
            }
            catch (NullReferenceException e)
            {
                return "";
            }
        }

        [HttpGet]
        public string GetRecentMessages(int id)
        {
            List<Message> recentMessages = _context.Message.Where(msg => msg.ConversationId == id)
                .OrderByDescending(msg => msg.Id).Take(50).ToList();

            return JsonConvert.SerializeObject(recentMessages);
        }

    }
}
