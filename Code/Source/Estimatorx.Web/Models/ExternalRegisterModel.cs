using System.ComponentModel.DataAnnotations;

namespace Estimatorx.Web.Models
{
    public class ExternalRegisterModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}