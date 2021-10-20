using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstimatorX.Service.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<Core.Entities.User> _userManager;

        public ConfirmEmailModel(UserManager<Core.Entities.User> userManager)
        {
            _userManager = userManager;
        }

        public bool Succeeded { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            if (userId == null || token == null)
                return LocalRedirect("/");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Page();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            Succeeded = result.Succeeded;

            return Page();
        }
    }
}