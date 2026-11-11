using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class EditUserTicketViewModel
    {
        public int Id { get; set; }

        // Solo para mostrar en el formulario
        public string TicketNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título es obligatorio")]
        [MaxLength(255)]
        [Display(Name = "Título")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        public string Description { get; set; } = string.Empty;
    }
}
