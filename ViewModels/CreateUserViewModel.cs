using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class CreateUserTicketViewModel
    {
        [Required(ErrorMessage = "El titulo es obligatorio")]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Description { get; set; } = string.Empty;
    }
}