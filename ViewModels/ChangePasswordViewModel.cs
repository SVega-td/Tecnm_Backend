using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage ="Requiere Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Requiere Contraseña")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Minimo 8 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [Compare("Confirmar contraseña", ErrorMessage ="No coincide contraseña")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage ="Requiere confirmar contreaseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirme la contraseña")]
        public string ConfirmPassword { get; set; }

    }
}
