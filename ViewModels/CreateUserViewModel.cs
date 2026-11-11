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


        // Solo para mostrar (no se guardan desde el formulario)
        public string TicketNumber { get; set; } = string.Empty;
   
        [Display(Name = "Estado")]
        public int? StatusId { get; set; }

        [Display(Name = "Prioridad")]
        public int? PriorityId { get; set; }

        [Display(Name = "Asignado a")]
        public string? AssignedToId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha límite")]
        public DateTime? DueDate { get; set; }
    }
}