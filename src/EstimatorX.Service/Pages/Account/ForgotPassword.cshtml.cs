using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EstimatorX.Core.Extensions;
using EstimatorX.Core.Models;
using EstimatorX.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace EstimatorX.Service.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<Core.Entities.User> _userManager;
        private readonly IEmailTemplateService _templateService;
        private readonly IOptions<DataProtectionTokenProviderOptions> _tokenOptions;

        public ForgotPasswordModel(UserManager<Core.Entities.User> userManager, IEmailTemplateService templateService, IOptions<DataProtectionTokenProviderOptions> tokenOptions)
        {
            _userManager = userManager;
            _templateService = templateService;
            _tokenOptions = tokenOptions;
        }

        [BindProperty]
        public InputModel Input { get; set; }

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

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { token },
                protocol: Request.Scheme);

            var model = Request.UserAgent<UserResetPasswordEmail>();

            model.RecipientName = user.DisplayName;
            model.RecipientAddress = user.Email;
            model.Link = resetLink;
            model.ExpireHours = (int)_tokenOptions.Value.TokenLifespan.TotalHours;

            var result = await _templateService.SendResetPasswordEmail(model);

            return RedirectToPage("./ForgotPasswordConfirmation");
        }


        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

    }
}
