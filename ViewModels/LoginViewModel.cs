using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class LoginViewModel
    {
        [Required (ErrorMessage = "Email requerido")]
        [EmailAddress]

        public string Email { get; set; }

        [Required(ErrorMessage = "Contraseña requerida")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [Display(Name = "RememberMe")]

        public bool RememberMe { get; set; }
    }
}
