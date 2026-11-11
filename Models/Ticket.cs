using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDeskWeb.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string TicketNumber { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Relación opcional a TicketStatus
        public int? StatusId { get; set; }
        public TicketStatus? Status { get; set; }

        // Relación opcional a TicketPriority
        public int? PriorityId { get; set; }
        public TicketPriority? Priority { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime? DueDate { get; set; }

        // Autor y relaciones a usuarios (todas opcionales)
        public string? CreatedById { get; set; }
        public string? AssignedToId { get; set; }
        public string? ResolvedById { get; set; }
        public string? ClosedById { get; set; }

        [ForeignKey(nameof(CreatedById))]
        public User? CreatedBy { get; set; }

        [ForeignKey(nameof(AssignedToId))]
        public User? AssignedTo { get; set; }

        [ForeignKey(nameof(ResolvedById))]
        public User? ResolvedBy { get; set; }

        [ForeignKey(nameof(ClosedById))]
        public User? ClosedBy { get; set; }
    }
}