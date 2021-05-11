using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using PlantTrade.Models;

namespace PlantTrade.Controllers
{
	public class FeedbackController : Controller
	{
		private readonly planttradeContext _context;
		private readonly UserManager<User> _UserManager;
		private readonly IEmailSender _emailSender;

		public FeedbackController(planttradeContext context, UserManager<User> UserManager, IEmailSender emailSender)
		{
			this._context = context;
			this._UserManager = UserManager;
			this._emailSender = emailSender;
		}


		public IActionResult Feedback(string page, string userId = "Guest")
		{
			Feedback feedback = new Feedback();
			feedback.Page = page;
			feedback.UserId = userId;
			return View(feedback);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateFeedback([Bind("Id,Page,FeedbackText,UserId")] Feedback feedback)
		{
			if (ModelState.IsValid)
			{
				_context.Add(feedback);
				await _context.SaveChangesAsync();
			}
			else
			{
				return NotFound();
			}
			return RedirectToAction("FeedbackConformation");
		}

		public IActionResult FeedbackConformation()
		{
			return View();
		}
	}
}
