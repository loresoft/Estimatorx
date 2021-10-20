using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EstimatorX.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Service.Pages.Account.Manage
{
    [Authorize]
    public class ProfileModel : PageModelBase
    {
        private readonly SignInManager<Core.Entities.User> _signInManager;
        private readonly UserManager<Core.Entities.User> _userManager;

        public ProfileModel(ILoggerFactory loggerFactory, SignInManager<Core.Entities.User> signInManager, UserManager<Core.Entities.User> userManager) : base(loggerFactory)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string Username { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var displayName = user.DisplayName;
            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                DisplayName = displayName,
                Email = email,
                PhoneNumber = phoneNumber,
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user '{User}'.");

            var userId = await _userManager.GetUserIdAsync(user);

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                CheckResult(setEmailResult, userId);
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                CheckResult(setPhoneResult, userId);
            }

            user.DisplayName = Input.DisplayName;
            var updateResult = await _userManager.UpdateAsync(user);
            CheckResult(updateResult, userId);

            await _signInManager.RefreshSignInAsync(user);
            ShowAlert("Your profile has been updated");

            return RedirectToPage();
        }

        private void CheckResult(IdentityResult identityResult, string userId)
        {
            if (identityResult.Succeeded)
                return;

            throw new InvalidOperationException($"Unexpected error occurred updating user with ID '{userId}'.");
        }


        public class InputModel
        {
            [Required]
            [Display(Name = "Display Name")]
            public string DisplayName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email Address")]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }
    }
}