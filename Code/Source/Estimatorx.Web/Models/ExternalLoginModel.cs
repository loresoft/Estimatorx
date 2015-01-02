using System.ComponentModel.DataAnnotations;

namespace Estimatorx.Web.Models
{
    public class ExternalLoginModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }
}