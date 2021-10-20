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
    public class SetPasswordModel : PageModelBase
    {
        private readonly UserManager<Core.Entities.User> _userManager;
        private readonly SignInManager<Core.Entities.User> _signInManager;

        public SetPasswordModel(ILoggerFactory loggerFactory, UserManager<Core.Entities.User> userManager, SignInManager<Core.Entities.User> signInManager) : base(loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (hasPassword)
                return RedirectToPage("./Password");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                var userId = _userManager.GetUserId(User);
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var changePasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);

            Logger.LogInformation("User set their password successfully.");
            ShowAlert("Your password has been changed");

            return RedirectToPage();
        }


        public class InputModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}