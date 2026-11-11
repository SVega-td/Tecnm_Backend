using System.Security.Claims;
using HelpDeskWeb.Data;
using HelpDeskWeb.Dtos.Tickets;
using HelpDeskWeb.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskWeb.Controllers.Api
{
    [ApiController]
    [Route("api/tickets")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TicketsController(AppDbContext db)
        {
            _db = db;
        }

        // CONSULTAR: solo tickets del usuario autenticado
        [HttpGet]
        public async Task<ActionResult<List<TicketResponse>>> GetMyTickets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var tickets = await _db.Tickets
                .AsNoTracking()
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Where(t => t.CreatedById == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TicketResponse
                {
                    Id = t.Id,
                    TicketNumber = t.TicketNumber,
                    Title = t.Title,
                    Description = t.Description,
                    StatusId = t.StatusId,
                    StatusName = t.Status != null ? t.Status.Name : null,
                    PriorityId = t.PriorityId,
                    PriorityName = t.Priority != null ? t.Priority.Name : null,
                    CreatedAt = t.CreatedAt,
                    DueDate = t.DueDate
                })
                .ToListAsync();

            return Ok(tickets);
        }

        // CONSULTAR por id: solo si el ticket pertenece al usuario autenticado
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TicketResponse>> GetMyTicketById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var ticket = await _db.Tickets
                .AsNoTracking()
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Where(t => t.Id == id && t.CreatedById == userId)
                .Select(t => new TicketResponse
                {
                    Id = t.Id,
                    TicketNumber = t.TicketNumber,
                    Title = t.Title,
                    Description = t.Description,
                    StatusId = t.StatusId,
                    StatusName = t.Status != null ? t.Status.Name : null,
                    PriorityId = t.PriorityId,
                    PriorityName = t.Priority != null ? t.Priority.Name : null,
                    CreatedAt = t.CreatedAt,
                    DueDate = t.DueDate
                })
                .FirstOrDefaultAsync();

            if (ticket is null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        // AGREGAR: ticket ligado al usuario autenticado
        [HttpPost]
        public async Task<ActionResult<TicketResponse>> CreateMyTicket([FromForm] CreateTicketRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var ticket = new Ticket
            {
                Title = request.Title,
                Description = request.Description,
                StatusId = request.StatusId,
                PriorityId = request.PriorityId,
                DueDate = request.DueDate,
                CreatedById = userId,
                CreatedAt = DateTime.UtcNow,
                TicketNumber = $"T-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..4].ToUpper()}"
            };

            _db.Tickets.Add(ticket);
            await _db.SaveChangesAsync();

            var created = await _db.Tickets
                .AsNoTracking()
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Where(t => t.Id == ticket.Id && t.CreatedById == userId)
                .Select(t => new TicketResponse
                {
                    Id = t.Id,
                    TicketNumber = t.TicketNumber,
                    Title = t.Title,
                    Description = t.Description,
                    StatusId = t.StatusId,
                    StatusName = t.Status != null ? t.Status.Name : null,
                    PriorityId = t.PriorityId,
                    PriorityName = t.Priority != null ? t.Priority.Name : null,
                    CreatedAt = t.CreatedAt,
                    DueDate = t.DueDate
                })
                .FirstAsync();

            return CreatedAtAction(nameof(GetMyTicketById), new { id = created.Id }, created);
        }
    }
}