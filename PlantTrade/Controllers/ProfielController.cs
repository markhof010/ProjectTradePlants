using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlantTrade.Models;
using PlantTrade.Services;
using Microsoft.AspNetCore.Identity;

namespace PlantTrade.Controllers
{
	public class ProfielController : Controller
	{

		private readonly planttradeContext _context;
		private readonly BlobService _blobService;
		private readonly UserManager<User> _UserManager;

		public ProfielController(planttradeContext context, BlobService blob, UserManager<User> UserManager)
		{
			this._context = context;
			this._blobService = blob;
			this._UserManager = UserManager;
		}

		
		public async Task<IActionResult> mijnItems()
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null)
			{
				return NotFound();
			}

			var loggedUserId = access.Id;
			var Items = await _context.Item.Where(m => m.UserId == loggedUserId).ToListAsync();
			if (Items == null)
			{
				return NotFound();
			}

			return View(Items);
		}

		
		public async Task<IActionResult> mijnTutorials()
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null)
			{
				return NotFound();
			}
			var loggedUserId = access.Id;
			var Tutorials = await _context.Tutorial.Where(m => m.UserId == loggedUserId).ToListAsync();
			if (Tutorials == null)
			{
				return NotFound();
			}
			return View(Tutorials);
		}

		public async Task<IActionResult> CreateTutorial()
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null)
			{
				return NotFound();
			}
			return View();
		}

		// POST: Tutorials/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateTut([Bind("Id,Title,Link,Available,UserId")] Tutorial tutorial)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				tutorial.UserId = _UserManager.GetUserId(User);
				if (tutorial.Link.Contains("youtube"))
				{
					if (tutorial.Link.Contains("watch?v="))
					{
						tutorial.Link = tutorial.Link.Replace("watch?v=", "embed/");
					}
					else
					{
						NotFound();
					}
				}
				else
				{
					NotFound();
				}
				_context.Add(tutorial);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(mijnTutorials));
			}
			ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", tutorial.UserId);
			return View(tutorial);
		}

		public async Task<IActionResult> CreateItem()
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null)
			{
				return NotFound();
			}
			
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Offer,Kind,Name,LatinName,SunHours,Water,Ground,TransactionKind,Picture,Description,UserId,Available,File")] Item item)
		{
			var loggedUser = _UserManager.GetUserId(User);
			if (ModelState.IsValid)
			{
				item.Available = 1;
				item.Picture = "PlaceHolder" + item.Name;
				item.UserId = loggedUser;

				_context.Add(item);
				await _context.SaveChangesAsync();

				var itemId = await _context.Item.FirstOrDefaultAsync(m => m.Picture == item.Picture);

				string ItemName = (itemId.Id).ToString();

				item.Picture = await _blobService.Upload(item.File, ItemName);

				try
				{
					_context.Update(item);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ItemExists(item.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(mijnItems));
			}
			ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", item.UserId);
			return View(item);
		}

		public async Task<IActionResult> mijnItem(int? id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null)
			{
				return NotFound();
			}

			if (id == null)
			{
				return NotFound();
			}

			var item = await _context.Item
				.FirstOrDefaultAsync(m => m.Id == id);
			if (item == null)
			{
				return NotFound();
			}

			return View(item);
		}

		public async Task<IActionResult> editItem(int? id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null)
			{
				return NotFound();
			}

			if (id == null)
			{
				return NotFound();
			}

			var item = await _context.Item.FindAsync(id);
			if (item == null)
			{
				return NotFound();
			}
			return View(item);
		}

		// POST: Items/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Offer,Kind,Name,LatinName,SunHours,Water,Ground,TransactionKind,Picture,Description,UserId,Available,File")] Item item)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null)
			{
				return NotFound();
			}
			
			if (id != item.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					if (item.File != null)
					{
						await _blobService.Delete(item.Picture);

						string ItemName = (item.Id).ToString();

						item.Picture = await _blobService.Upload(item.File, ItemName);
					}
					_context.Update(item);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ItemExists(item.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(mijnItem), new { id = item.Id});
			}
			return View(item);
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> deleteItem(int id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null)
			{
				return NotFound();
			}

			var item = await _context.Item.FindAsync(id);
			
			await _blobService.Delete(item.Picture);

			_context.Item.Remove(item);

			var reports = await _context.Report.Where(m => m.Kind == "Item" && m.ReportId == id.ToString()).ToListAsync();

			if (reports != null)
			{
				foreach (var report in reports) 
				{
					_context.Report.Remove(report);
				}
			}

			var conversations = await _context.Conversation.Where(m => m.ItemId == id).ToListAsync();
			if (conversations != null)
			{
				foreach (var conversation in conversations)
				{
					var messages = await _context.Message.Where(m => m.ConversationId == conversation.Id).ToListAsync();
					if (messages != null)
					{
						foreach (var message in messages)
						{
							_context.Message.Remove(message);
						}
					}
					_context.Conversation.Remove(conversation);
				}
			}

			

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(mijnItems));
		}

		// POST: Tutorials/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> deleteTutorial(int id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null)
			{
				return NotFound();
			}

			var tutorial = await _context.Tutorial.FindAsync(id);

			_context.Tutorial.Remove(tutorial);

			var reports = await _context.Report.Where(m => m.Kind == "Tutorial" && m.ReportId == id.ToString()).ToListAsync();

			if (reports != null)
			{
				foreach (var report in reports)
				{
					_context.Report.Remove(report);
				}
			}

			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(mijnTutorials));
		}

		public async Task<IActionResult> editTutorial(int? id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (id == null || access == null)
			{
				return NotFound();
			}

			var tutorial = await _context.Tutorial.FindAsync(id);
			if (tutorial == null)
			{
				return NotFound();
			}

			tutorial.Link = tutorial.Link.Replace("embed/", "watch?v=");

			return View(tutorial);
		}

		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> editTut(int id, [Bind("Id,Title,Link,Available,UserId")] Tutorial tutorial)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (id != tutorial.Id || access == null)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				if (tutorial.Link.Contains("youtube"))
				{
					if (tutorial.Link.Contains("watch?v="))
					{
						tutorial.Link = tutorial.Link.Replace("watch?v=", "embed/");
					}
					else
					{
						NotFound();
					}
				}
				else
				{
					NotFound();
				}
				_context.Tutorial.Update(tutorial);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(mijnTutorials));
			}
			return View(tutorial);
		}


		public async void deleteAccount(string id)
		{
			var items = await _context.Item.Where(m => m.UserId == id).ToListAsync();
			var tutorials = await _context.Tutorial.Where(m => m.UserId == id).ToListAsync();
			var conversations = await _context.Conversation.Where(m => m.UserId == id || m.TraderId == id).Include(m => m.Message).ToListAsync();
			var reports = await _context.Report.Where(m => m.ReporterId == id).ToListAsync();
			var reported = await _context.Report.Where(m => m.ReportId == id && m.Kind == "Person").ToListAsync();

			_context.RemoveRange(items);
			_context.RemoveRange(tutorials);
			foreach (Conversation conversation in conversations)
			{
				_context.RemoveRange(conversation.Message);
			}
			_context.RemoveRange(conversations);
			_context.RemoveRange(reports);
			_context.RemoveRange(reported);

			await _context.SaveChangesAsync();
		}

		private bool ItemExists(int id)
		{
			return _context.Item.Any(e => e.Id == id);
		}

	}
}
