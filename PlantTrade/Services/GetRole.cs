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
namespace PlantTrade.Services
{
	public class GetRole
	{
		private readonly planttradeContext _context;
		private readonly UserManager<User> _UserManager;

		public GetRole(planttradeContext context, UserManager<User> UserManager)
		{
			this._context = context;
			this._UserManager = UserManager;
		}

		public async Task<string> GetRoleAsync(string id)
		{
			var user = await _context.User.Where(m => m.Id == id).FirstOrDefaultAsync();

			return user.Role;
		}
		public bool role(string id)
		{
			var role = GetRoleAsync(id).Result;

			bool check = role == "Admin";

			return check;
		}

	}
}
