using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nombre requerido")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email requerido")]
        [EmailAddress]

        public string Email { get; set; }

        [Required(ErrorMessage = "Contraseña requerida")]
        [StringLength(40, MinimumLength =8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType(DataType.Password)]
        [Compare("Confirm Password", ErrorMessage = "No coincide")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Requiere confirmar contraseña")]
        [DataType(DataType.Password)]
        [Display(Name ="Confirma contraseña")]
        public string ConfirmPassword { get; set; }

    }
}
