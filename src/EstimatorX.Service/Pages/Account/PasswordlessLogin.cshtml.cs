using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstimatorX.Core.Entities;
using EstimatorX.Service.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Service.Pages.Account
{
    public class PasswordlessLoginModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<PasswordlessLoginModel> _logger;

        public PasswordlessLoginModel(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<PasswordlessLoginModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public bool Succeeded { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            if (string.IsNullOrEmpty(token))
                return RedirectToPage("/Account/Login");
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Account/Login");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Page();

            var isValid = await _userManager.VerifyUserPasswordlessTokenAsync(user, token);
            if (!isValid)
                return Page();

            await _signInManager.SignInAsync(user, true);
            Succeeded = isValid;

            _logger.LogInformation("User '{email}' logged in with link.", user.Email);

            return LocalRedirect("/");
        }
    }
}
