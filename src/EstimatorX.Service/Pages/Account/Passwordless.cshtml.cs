using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EstimatorX.Core.Extensions;
using EstimatorX.Core.Models;
using EstimatorX.Core.Services;
using EstimatorX.Service.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace EstimatorX.Service.Pages.Account
{
    [AllowAnonymous]
    public class PasswordlessModel : PageModel
    {
        private readonly UserManager<Core.Entities.User> _userManager;
        private readonly IEmailTemplateService _templateService;
        private readonly IOptions<PasswordlessLoginProviderOptions> _options;


        public PasswordlessModel(UserManager<Core.Entities.User> userManager, IEmailTemplateService templateService, IOptions<PasswordlessLoginProviderOptions> options)
        {
            _userManager = userManager;
            _templateService = templateService;
            _options = options;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }


        public async Task OnGetAsync()
        {
            ReturnUrl = ReturnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError("Input.Email", "Email address not registered.");
                return Page();
            }

            var token = await _userManager.GenerateUserPasswordlessTokenAsync(user);

            await SendEmail(user, token);

            return RedirectToPage("./PasswordlessConfirmation");
        }

        private async Task SendEmail(Core.Entities.User user, string token)
        {
            var userId = user.Id;
            var loginLink = Url.Page(
                "/Account/PasswordlessLogin",
                pageHandler: null,
                values: new { token, userId },
                protocol: Request.Scheme);

            var model = Request.UserAgent<UserPasswordlessEmail>();
            model.RecipientName = user.DisplayName;
            model.RecipientAddress = user.Email;
            model.Link = loginLink;
            model.ExpireMinutes = (int)_options.Value.TokenLifespan.TotalMinutes;

            var result = await _templateService.SendPasswordlessLoginEmail(model);
        }


        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
    }
}