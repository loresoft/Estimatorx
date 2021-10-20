using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstimatorX.Service.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<Core.Entities.User> _userManager;

        public ResetPasswordModel(UserManager<Core.Entities.User> userManager)
        {
            _userManager = userManager;
            Input = new InputModel();
        }

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public IActionResult OnGet()
        {
            if (Token == null)
                return BadRequest("A token must be supplied for password reset.");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError("Input.Email", "Email address not registered.");
                return Page();
            }

            var result = await _userManager.ResetPasswordAsync(user, Token, Input.Password);
            if (result.Succeeded)
                return RedirectToPage("./ResetPasswordConfirmation");

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }


        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

    }
}
