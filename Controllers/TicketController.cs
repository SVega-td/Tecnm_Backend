using HelpDeskWeb.Data;
using HelpDeskWeb.Models;
using HelpDeskWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HelpDeskWeb.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public TicketController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // Usuarios con rol Administrador (para el combo "Asignado a")
        private List<User> GetAdminUsers()
        {
            return (from user in _appDbContext.Users
                    join userRole in _appDbContext.UserRoles on user.Id equals userRole.UserId
                    join role in _appDbContext.Roles on userRole.RoleId equals role.Id
                    where role.Name == "Administrador"
                    orderby user.FullName
                    select user)
                    .Distinct()
                    .ToList();
        }

        private async Task LoadListsAsync(int? statusId, int? priorityId, string? assignedToId)
        {
            ViewBag.Statuses = new SelectList(
                await _appDbContext.TicketStatus.OrderBy(s => s.Name).ToListAsync(),
                "Id", "Name", statusId);

            ViewBag.Priorities = new SelectList(
                await _appDbContext.TicketPriorities.OrderBy(p => p.Name).ToListAsync(),
                "Id", "Name", priorityId);

            ViewBag.Admins = new SelectList(GetAdminUsers(), "Id", "FullName", assignedToId);
        }

        // ---------- USUARIO ----------

        [Authorize(Roles = "Usuario")]
        [HttpGet]
        public async Task<IActionResult> MyTickets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _appDbContext.Tickets
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Where(t => t.CreatedById == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return View(list);
        }

        [Authorize(Roles = "Usuario")]
        [HttpGet]
        public IActionResult CreateUserTicket()
        {
            return View(new CreateUserTicketViewModel());
        }

        [Authorize(Roles = "Usuario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserTicket(CreateUserTicketViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Forbid();

            var ticket = new Ticket
            {
                Title = model.Title,
                Description = model.Description,
                TicketNumber = $"T-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}",
                CreatedById = userId,
                CreatedAt = DateTime.UtcNow
            };

            _appDbContext.Tickets.Add(ticket);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(MyTickets));
        }

        [Authorize(Roles = "Usuario")]
        [HttpGet]
        public async Task<IActionResult> EditUserTicket(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Solo puede editar un ticket que él mismo creó
            var ticket = await _appDbContext.Tickets
                .FirstOrDefaultAsync(t => t.Id == id && t.CreatedById == userId);

            if (ticket == null) return NotFound();

            var model = new EditUserTicketViewModel
            {
                Id = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                Title = ticket.Title,
                Description = ticket.Description
            };

            return View(model);
        }

        [Authorize(Roles = "Usuario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserTicket(EditUserTicketViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ticket = await _appDbContext.Tickets
                .FirstOrDefaultAsync(t => t.Id == model.Id && t.CreatedById == userId);

            if (ticket == null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.TicketNumber = ticket.TicketNumber;
                return View(model);
            }

            ticket.Title = model.Title;
            ticket.Description = model.Description;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();

            TempData["Success"] = "El ticket se actualizó correctamente.";
            return RedirectToAction(nameof(MyTickets));
        }

        [Authorize(Roles = "Usuario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserTicket(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Solo puede eliminar un ticket que él mismo creó
            var ticket = await _appDbContext.Tickets
                .FirstOrDefaultAsync(t => t.Id == id && t.CreatedById == userId);

            if (ticket == null) return NotFound();

            _appDbContext.Tickets.Remove(ticket);
            await _appDbContext.SaveChangesAsync();

            TempData["Success"] = "El ticket se eliminó correctamente.";
            return RedirectToAction(nameof(MyTickets));
        }

        // ---------- ADMIN + USUARIO ----------

        [Authorize(Roles = "Administrador,Usuario")]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _appDbContext.Tickets
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.ResolvedBy)
                .Include(t => t.ClosedBy)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null) return NotFound();

            // Un usuario normal solo puede ver sus propios tickets
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!User.IsInRole("Administrador") && ticket.CreatedById != userId)
            {
                return Forbid();
            }

            return View(ticket);
        }

        // ---------- ADMINISTRADOR ----------

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await _appDbContext.Tickets
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return View(list);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _appDbContext.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            var model = new EditTicketViewModel
            {
                Id = ticket.Id,
                TicketNumber = ticket.TicketNumber,
                Title = ticket.Title,
                StatusId = ticket.StatusId,
                PriorityId = ticket.PriorityId,
                AssignedToId = ticket.AssignedToId,
                DueDate = ticket.DueDate
            };

            await LoadListsAsync(model.StatusId, model.PriorityId, model.AssignedToId);
            return View(model);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditTicketViewModel model)
        {
            var ticket = await _appDbContext.Tickets.FindAsync(model.Id);
            if (ticket == null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.TicketNumber = ticket.TicketNumber;
                model.Title = ticket.Title;
                await LoadListsAsync(model.StatusId, model.PriorityId, model.AssignedToId);
                return View(model);
            }

            ticket.StatusId = model.StatusId;
            ticket.PriorityId = model.PriorityId;
            ticket.AssignedToId = string.IsNullOrEmpty(model.AssignedToId) ? null : model.AssignedToId;
            ticket.DueDate = model.DueDate;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = ticket.Id });
        }
    }
}
