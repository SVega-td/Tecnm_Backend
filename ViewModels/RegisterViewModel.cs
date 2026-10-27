using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nombre requerido")]
        public required string Name { get; set; }
        
        [Required(ErrorMessage = "Email requerido")]
        [EmailAddress(ErrorMessage = "Formato de email no válido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Contraseña requerida")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Requiere confirmar contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirma contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public required string ConfirmPassword { get; set; }
    }
}