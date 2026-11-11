using Microsoft.AspNetCore.Mvc;

namespace HelpDeskWeb.Dtos.Tickets
{
    public class TicketResponse : Controller
    {
            public int Id { get; set; }
            public string TicketNumber { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public int? StatusId { get; set; }
            public string? StatusName { get; set; }
            public int? PriorityId { get; set; }
            public string? PriorityName { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? DueDate { get; set; }
    }
}
