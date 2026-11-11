using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Requiere Email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Requiere Contraseña")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Requiere confirmar contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirme la contraseña")]
        [Compare("NewPassword", ErrorMessage = "No coincide contraseña")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}