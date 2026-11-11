using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.ViewModels
{
    public class EditTicketViewModel
    {
        public int Id { get; set; }

        // Solo para mostrar (no se guardan desde el formulario)
        public string TicketNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public int? StatusId { get; set; }

        [Display(Name = "Prioridad")]
        public int? PriorityId { get; set; }

        [Display(Name = "Asignado a")]
        public string? AssignedToId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha límite")]
        public DateTime? DueDate { get; set; }
        public string Description { get; internal set; }
    }
}