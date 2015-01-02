using System.ComponentModel.DataAnnotations;

namespace Estimatorx.Web.Models
{
    public class EmailModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}