using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Requiere Email")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
