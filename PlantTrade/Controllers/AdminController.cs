using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlantTrade.Models;

namespace PlantTrade.Controllers
{
	public class AdminController : Controller
	{
		private readonly planttradeContext _context;
		private readonly UserManager<User> _UserManager;
		private readonly IEmailSender _emailSender;

		public AdminController(planttradeContext context, UserManager<User> UserManager, IEmailSender emailSender)
		{
			_context = context;
			this._UserManager = UserManager;
			this._emailSender = emailSender;
		}

		//Actions for Admin rights
		public async Task<IActionResult> AdminRights()
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			var users = await _context.User.ToListAsync();

			return View(users);
		}

		[HttpPost]
		public async Task<IActionResult> AddAdminRights(string email)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			var user = await _context.User.Where(m => m.Email == email).FirstOrDefaultAsync();
			user.Role = "Admin";
			_context.User.Update(user);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(AdminRights));
		}

		public async Task<IActionResult> DeleteRights(string id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			var user = await _context.User.Where(m => m.Id == id).FirstOrDefaultAsync();

			user.Role = null;
			_context.User.Update(user);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(AdminRights));
		}

		//Actions for AdminReport
		public async Task<IActionResult> AdminReport()
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			List<AdminReport> adminReports = new List<AdminReport>();
			var reports = await _context.Report.Include(i => i.User).ToListAsync();
			while (reports.Count() > 0)
			{
				var adReports = reports.FindAll(m => m.ReportId == reports[0].ReportId && m.Kind == reports[0].Kind);
				reports.RemoveAll(m => m.ReportId == adReports[0].ReportId);

				AdminReport adminReport = new AdminReport();
				adminReport.Count = adReports.Count();
				adminReport.Reports = adReports;
				adminReport.Kind = adReports[0].Kind;

				if (adReports[0].Kind == "Item")
				{
					var item = await _context.Item.Where(m => m.Id == Int32.Parse(adReports[0].ReportId)).FirstOrDefaultAsync();
					adminReport.Name = item.Name;
				}
				else if (adReports[0].Kind == "Tutorial")
				{
					var item = await _context.Tutorial.Where(m => m.Id == Int32.Parse(adReports[0].ReportId)).FirstOrDefaultAsync();
					adminReport.Name = item.Title;
				}
				else if (adReports[0].Kind == "Person")
				{
					var item = await _context.User.Where(m => m.Id == adReports[0].ReportId).FirstOrDefaultAsync();
					adminReport.Name = item.UserName;
				}
				adminReports.Add(adminReport);
			}
			return View(adminReports);
		}

		public async Task<IActionResult> DeleteReport(int id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			var report = await _context.Report.FindAsync(id);
			_context.Report.Remove(report);
			await _context.SaveChangesAsync();
			if (report.Kind == "Item")
			{
				return RedirectToAction(nameof(AdminReportDetailsItem), new { id = report.ReportId });
			}
			else if (report.Kind == "Tutorial")
			{
				return RedirectToAction(nameof(AdminReportDetailsTutorial), new { id = report.ReportId });
			}
			else if (report.Kind == "Person")
			{
				return RedirectToAction(nameof(AdminReportDetailsProfiel), new { id = report.ReportId });
			}
			return RedirectToAction(nameof(AdminReport));
		}

		//Actions for AdminReportDetailsItem
		public async Task<IActionResult> AdminReportDetailsItem(string id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			AdminReport adminReport = new AdminReport();

			adminReport.Reports = await _context.Report.Where(m => m.Kind == "Item" && m.ReportId == id).Include(m => m.User).ToListAsync();
			adminReport.Count = adminReport.Reports.Count();
			adminReport.Item = await _context.Item.Where(m => m.Id == Int32.Parse(id)).FirstOrDefaultAsync();

			return View(adminReport);
		}

		public async Task<IActionResult> restoreReportedItem(int id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			var reports = await _context.Report.Where(m => m.ReportId == id.ToString() && m.Kind == "Item").ToListAsync();
			var item = await _context.Item.Where(m => m.Id == id).FirstOrDefaultAsync();
			var userReported = await _context.User.Where(m => m.Id == item.UserId).FirstOrDefaultAsync();

			foreach (Report report in reports)
			{
				_context.Report.Remove(report);
			}

			item.Available = 1;
			_context.Item.Update(item);

			await _context.SaveChangesAsync();

			await _emailSender.SendEmailAsync(userReported.Email, "Item is hersteld",
								$"Beste {userReported.UserName},<br/><br/>" +
								$"Uw item \"{item.Name}\" is hersteld door de administrator.<br/><br/>" +
								$"Vriendelijke groeten,<br/>StekOverflow"
								);

			return RedirectToAction(nameof(AdminReport));
		}

		//Actions for AdminReportDetailsTutorial
		public async Task<IActionResult> AdminReportDetailsTutorial(string id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			AdminReport adminReport = new AdminReport();

			adminReport.Reports = await _context.Report.Where(m => m.Kind == "Tutorial" && m.ReportId == id).Include(m => m.User).ToListAsync();
			adminReport.Count = adminReport.Reports.Count();
			adminReport.Tutorial = await _context.Tutorial.Where(m => m.Id == Int32.Parse(id)).FirstOrDefaultAsync();

			return View(adminReport);
		}

		public async Task<IActionResult> restoreReportedTutorial(int id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			var reports = await _context.Report.Where(m => m.ReportId == id.ToString() && m.Kind == "Tutorial").ToListAsync();
			var tutorial = await _context.Tutorial.Where(m => m.Id == id).FirstOrDefaultAsync();
			var userReported = await _context.User.Where(m => m.Id == tutorial.UserId).FirstOrDefaultAsync();

			foreach (Report report in reports)
			{
				_context.Report.Remove(report);
			}

			tutorial.Available = 1;
			_context.Tutorial.Update(tutorial);

			await _context.SaveChangesAsync();

			await _emailSender.SendEmailAsync(userReported.Email, "Tutorial is hersteld",
								$"Beste {userReported.UserName},<br/><br/>" +
								$"Uw tutorial \"{tutorial.Title}\" is hersteld door de administrator.<br/><br/>" +
								$"Vriendelijke groeten,<br/>StekOverflow"
								);

			return RedirectToAction(nameof(AdminReport));
		}

		//Actions for AdminReportDetailsProfiel
		public async Task<IActionResult> AdminReportDetailsProfiel(string id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			AdminReport adminReport = new AdminReport();

			adminReport.Reports = await _context.Report.Where(m => m.Kind == "Person" && m.ReportId == id).Include(m => m.User).ToListAsync();
			adminReport.Count = adminReport.Reports.Count();
			adminReport.Profile = await _context.User.Where(m => m.Id == id).Include(m => m.Item).Include(m => m.Tutorial).FirstOrDefaultAsync();

			return View(adminReport);
		}

		public async Task<IActionResult> restoreReportedProfiel(string id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			var reports = await _context.Report.Where(m => m.ReportId == id.ToString() && m.Kind == "Person").ToListAsync();
			var reportedUser = await _context.User.Where(m => m.Id == id).Include(m => m.Item).Include(m => m.Tutorial).FirstOrDefaultAsync();

			foreach (Report report in reports)
			{
				_context.Report.Remove(report);
			}
			foreach (Item item in reportedUser.Item)
			{
				item.Available = 1;
				_context.Item.Update(item);
			}
			foreach (Tutorial tutorial in reportedUser.Tutorial)
			{
				tutorial.Available = 1;
				_context.Tutorial.Update(tutorial);
			}

			reportedUser.EmailConfirmed = true;
			_context.User.Update(reportedUser);

			await _context.SaveChangesAsync();

			await _emailSender.SendEmailAsync(reportedUser.Email, "Account is hersteld",
								$"Beste {reportedUser.UserName},<br/><br/>" +
								$"Uw account is hersteld door de administrator.<br/><br/>" +
								$"Vriendelijke groeten,<br/>StekOverflow"
								);

			return RedirectToAction(nameof(AdminReport));
		}

		//Actions for AdminItems
		public async Task<IActionResult> AdminItems()
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}
			var items = await _context.Item.ToListAsync();
			return View(items);
		}

		public async Task<IActionResult> AdminItemsDetails(int id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}
			var item = await _context.Item.Where(m => m.Id == id).FirstOrDefaultAsync();
			return View(item);
		}

		//Actions for adminTutorials
		public async Task<IActionResult> AdminTutorials()
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}
			var Tutorials = await _context.Tutorial.ToListAsync();
			return View(Tutorials);
		}

		//Actions for AdminProfiles
		public async Task<IActionResult> AdminProfiles()
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}
			var users = await _context.User.ToListAsync();
			return View(users);
		}

		public async Task<IActionResult> AdminProfileDetails(string id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}
			var user = await _context.User.Where(m => m.Id == id).Include(m => m.Item).Include(m => m.Tutorial).FirstOrDefaultAsync();
			return View(user);
		}

		//Actions for AdminFeedback
		public async Task<IActionResult> AdminFeedback()
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}
			var feedback = await _context.Feedback.ToListAsync();
			return View(feedback);
		}

		public async Task<IActionResult> DeleteFeedback(int id)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}
			var feedback = await _context.Feedback.Where(m => m.Id == id).FirstOrDefaultAsync();
			_context.Feedback.Remove(feedback);
			await _context.SaveChangesAsync();
			return Redirect(nameof(AdminFeedback));
		}

		//Actions to delete
		public async Task<IActionResult> deleteItem(int id, string page)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			var reports = await _context.Report.Where(m => m.ReportId == id.ToString() && m.Kind == "Item").ToListAsync();
			var item = await _context.Item.Where(m => m.Id == id).FirstOrDefaultAsync();
			var conversations = await _context.Conversation.Where(m => m.ItemId == id).Include(m => m.Message).ToListAsync();
			var userReported = await _context.User.Where(m => m.Id == item.UserId).FirstOrDefaultAsync();

			foreach (Conversation conversation in conversations)
			{

				foreach (Message message in conversation.Message)
				{
					_context.Message.Remove(message);
				}
				_context.Conversation.Remove(conversation);
			}

			foreach (Report report in reports)
			{
				_context.Report.Remove(report);
			}

			_context.Item.Remove(item);
			await _context.SaveChangesAsync();

			await _emailSender.SendEmailAsync(userReported.Email, "Item is verwijderd",
								$"Beste {userReported.UserName},<br/><br/>" +
								$"Uw item \"{item.Name}\" is verwijderd door de administrator vanwege ongepaste content.<br/><br/>" +
								$"Vriendelijke groeten,<br/>StekOverflow"
								);
			if (page == "AdminItems")
			{
				return RedirectToAction(nameof(AdminItems));
			}
			else
			{
				return RedirectToAction(nameof(AdminReport));
			}
		}

		public async Task<IActionResult> deleteTutorial(int id, string page)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			var reports = await _context.Report.Where(m => m.ReportId == id.ToString() && m.Kind == "Tutorial").ToListAsync();
			var tutorial = await _context.Tutorial.Where(m => m.Id == id).FirstOrDefaultAsync();
			var userReported = await _context.User.Where(m => m.Id == tutorial.UserId).FirstOrDefaultAsync();


			foreach (Report report in reports)
			{
				_context.Report.Remove(report);
			}

			_context.Tutorial.Remove(tutorial);
			await _context.SaveChangesAsync();

			await _emailSender.SendEmailAsync(userReported.Email, "Tutorial is verwijderd",
								$"Beste {userReported.UserName},<br/><br/>" +
								$"Uw tutorial \"{tutorial.Title}\" is verwijderd door de administrator vanwege ongepaste content.<br/><br/>" +
								$"Vriendelijke groeten,<br/>StekOverflow"
								);

			if (page == "AdminTutorials")
			{
				return RedirectToAction(nameof(AdminTutorials));
			}
			else
			{
				return RedirectToAction(nameof(AdminReport));
			}
			
		}

		public async Task<IActionResult> deleteProfile(string id, string page)
		{
			var access = await _UserManager.GetUserAsync(User);
			if (access == null || access.Role != "Admin")
			{
				return NotFound();
			}

			var reports = await _context.Report.Where(m => m.ReportId == id && m.Kind == "Person").ToListAsync();
			var reported = await _context.Report.Where(m => m.ReporterId == id).ToListAsync();
			var user = await _context.User.Where(m => m.Id == id).Include(m => m.Tutorial).Include(m => m.Item).Include(m => m.ConversationUser).ThenInclude(m => m.Message).Include(m => m.ConversationTrader).ThenInclude(m => m.Message).FirstOrDefaultAsync();

			//remove reports
			foreach (Report report in reports)
			{
				_context.Report.Remove(report);
			}

			//remove conversations and messages
			foreach (Conversation conversation in user.ConversationUser)
			{
				foreach (Message message in conversation.Message)
				{
					_context.Message.Remove(message);
				}
				_context.Conversation.Remove(conversation);
			}

			//remove conversations and messages
			foreach (Conversation conversation in user.ConversationTrader)
			{
				foreach (Message message in conversation.Message)
				{
					_context.Message.Remove(message);
				}
				_context.Conversation.Remove(conversation);
			}

			//remove items
			foreach (Item item in user.Item)
			{
				var reportsItem = await _context.Report.Where(m => m.ReportId == item.Id.ToString() && m.Kind == "Item").ToListAsync();
				foreach (Report report in reportsItem)
				{
					_context.Report.Remove(report);
				}
				_context.Item.Remove(item);
			}

			//remove tutorials
			foreach (Tutorial tutorial in user.Tutorial)
			{
				var reportsTutorial = await _context.Report.Where(m => m.ReportId == tutorial.Id.ToString() && m.Kind == "Tutorial").ToListAsync();
				foreach (Report report in reportsTutorial)
				{
					_context.Report.Remove(report);
				}
				_context.Tutorial.Remove(tutorial);
			}

			_context.User.Remove(user);

			await _context.SaveChangesAsync();

			await _emailSender.SendEmailAsync(user.Email, "Account is verwijderd",
								$"Beste {user.UserName},<br/><br/>" +
								$"Uw account is verwijderd door de administrator vanwege ongepaste content.<br/><br/>" +
								$"Vriendelijke groeten,<br/>StekOverflow"
								);
			if(page == "AdminProfiles")
			{
				return RedirectToAction(nameof(AdminProfiles));
			}
			else
			{
				return RedirectToAction(nameof(AdminReport));
			}
		}
	}
}
