using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskWeb.Dtos.Tickets
{
    public class CreateTicketRequest
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public int? StatusId { get; set; }
        public int? PriorityId { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
