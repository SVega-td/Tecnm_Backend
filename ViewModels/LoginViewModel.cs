using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class LoginViewModel
    {
        [Required (ErrorMessage = "Email requerido")]
        [EmailAddress(ErrorMessage = "Formato de email no válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contraseña requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
