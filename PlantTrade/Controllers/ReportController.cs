using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlantTrade.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace PlantTrade.Controllers
{
	public class ReportController : Controller
	{
		private readonly planttradeContext _context;
		private readonly UserManager<User> _UserManager;
		private readonly IEmailSender _emailSender;

		public ReportController(planttradeContext context, UserManager<User> UserManager, IEmailSender emailSender)
		{
			this._context = context;
			this._UserManager = UserManager;
			this._emailSender = emailSender;
		}

		public IActionResult AlreadyReported(string kind, string reportId)
		{
			var loggedUser = _UserManager.GetUserId(User);
			if (loggedUser == null)
			{
				return NotFound();
			}
			Report report = new Report();
			report.Kind = kind;
			
			return View(report);
		}

		public async Task<IActionResult> ReportPage(string kind, string reportId)
		{
			var loggedUser = _UserManager.GetUserId(User);
			if (loggedUser == null)
			{
				return NotFound();
			}
			Report report = new Report();
			report.Kind = kind;
			report.ReportId = reportId;
			var exists = await _context.Report.AnyAsync(m => m.Kind == report.Kind && m.ReportId == report.ReportId && m.ReporterId == loggedUser);
			if (exists)
			{
				return RedirectToAction("AlreadyReported", new { kind = report.Kind});
			}
			return View(report);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateReport([Bind("Id,Kind,Description,ReportId,ReporterId")] Report report)
		{
			var loggedUser = _UserManager.GetUserId(User);
			if (ModelState.IsValid)
			{
				var exists = await _context.Report.AnyAsync(m => m.Kind == report.Kind && m.ReportId == report.ReportId && m.ReporterId == loggedUser);
				if (!exists)
				{
					report.ReporterId = loggedUser;
					_context.Add(report);
					await _context.SaveChangesAsync();

					var count = await _context.Report.Where(m => m.Kind == report.Kind && m.ReportId == report.ReportId).CountAsync();
					if (count > 2)
					{
						// This handles reports for the Items
						if (report.Kind == "Item") {
							var reportitem = await _context.Item.Where(m => m.Id == Int32.Parse(report.ReportId)).Include(m => m.User).FirstOrDefaultAsync();
							reportitem.Available = 0;
							_context.Update(reportitem);
							await _context.SaveChangesAsync();


							await _emailSender.SendEmailAsync(reportitem.User.Email, "Item geraporteerd", 
								$"Beste {reportitem.User.UserName},<br/><br/>" +
								$"{reportitem.Name} is geblokkeerd vanwege meerdere raporten.<br/>" +
								$"De administrator zal kijken naar de item kijken en beslissen of het nog op de site mag blijven.<br/>" +
								$"U zal een mail krijgen zodra de administrator dit behandeld heeft.<br/><br/>" +
								$"Vriendelijke groeten,<br/>StekOverflow"
								);
						}

						// This handles reports for the Tutorials
						else if (report.Kind == "Tutorial")
						{
							var reportTutorial = await _context.Tutorial.Where(m => m.Id == Int32.Parse(report.ReportId)).Include(m => m.User).FirstOrDefaultAsync();
							reportTutorial.Available = 0;
							_context.Update(reportTutorial);
							await _context.SaveChangesAsync();


							await _emailSender.SendEmailAsync(reportTutorial.User.Email, "Tutorial geraporteerd",
								$"Beste {reportTutorial.User.UserName},<br/><br/>" +
								$"{reportTutorial.Title} is geblokkeerd vanwege meerdere raporten.<br/>" +
								$"De administrator zal kijken naar de Tutorial kijken en beslissen of het nog op de site mag blijven.<br/>" +
								$"U zal een mail krijgen zodra de administrator dit behandeld heeft.<br/><br/>" +
								$"Vriendelijke groeten,<br/>StekOverflow"
								);
						}

						// This handles reports for the Persons
						else if (report.Kind == "Person")
						{
							var reportPerson = await _context.User.Where(m => m.Id == report.ReportId).FirstOrDefaultAsync();
							reportPerson.EmailConfirmed = false;
							_context.Update(reportPerson);

							var PersonItems = await _context.Item.Where(m => m.UserId == report.ReportId).ToListAsync();

							foreach (var item in PersonItems) {
								item.Available = 0;
							}

							var PersonTutorials = await _context.Tutorial.Where(m => m.UserId == report.ReportId).ToListAsync();
							foreach (var tutorial in PersonTutorials)
							{
								tutorial.Available = 0;
							}

							await _context.SaveChangesAsync();


							await _emailSender.SendEmailAsync(reportPerson.Email, "Profiel geraporteerd",
								$"Beste {reportPerson.UserName},<br/><br/>" +
								$"Uw account is geblokkeerd vanwege meerdere raporten.<br/>" +
								$"De administrator zal kijken naar uw profiel kijken en beslissen of het nog op de site mag blijven.<br/>" +
								$"U zal een mail krijgen zodra de administrator dit behandeld heeft.<br/><br/>" +
								$"Vriendelijke groeten,<br/>StekOverflow"
								);
						}
						// If the kind can not be found in the if statements
						else
						{
							return NotFound();
						}
					}
				}
			}
			return RedirectToAction("index", "Home");
		}
	}
}
