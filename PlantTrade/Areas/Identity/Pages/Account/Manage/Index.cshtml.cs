using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantTrade.Models;

namespace PlantTrade.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [StringLength(32, MinimumLength = 6)]
            [Display(Name = "Gebruikersnaam")]
            public string Username { get; set; }

            [StringLength(6,MinimumLength =6)]
            [Display(Name = "Postcode")]
            public string PostCode { get; set; }

            [Display(Name = "Specialisme")]
            public string Specialisme { get; set; }

            [Display(Name = "Over")]
            public string About { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            ClaimsPrincipal test = new ClaimsPrincipal();
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var currentUser = await _userManager.GetUserAsync(User);
            var postCode = currentUser.Postcode;
            var specialisme = currentUser.Specialisme;
            var about = currentUser.About;

            //Username = userName;

            Input = new InputModel
            {
                Username = userName,
                PostCode = postCode,
                Specialisme = specialisme,
                About = about
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            
            var updatedUser = await _userManager.GetUserAsync(User);
            updatedUser.UserName = Input.Username;
            updatedUser.Postcode = Input.PostCode;
            updatedUser.About = Input.About;
            updatedUser.Specialisme = Input.Specialisme;
            

            await _userManager.UpdateAsync(updatedUser);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Je profiel is geupdate.";
            return RedirectToPage();
        }
    }
}
