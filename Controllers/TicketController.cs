using HelpDeskWeb.Data;
using HelpDeskWeb.Models;
using HelpDeskWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HelpDeskWeb.Controllers
{
    public class TicketController : Controller
    {
        private AppDbContext _appDbContext;
        public TicketController(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        private List<User> GetAdminUsers()
        {
            return (from user in _appDbContext.Users
                    join userRole in _appDbContext.UserRoles on user.Id equals userRole.UserId
                    join role in _appDbContext.Roles on userRole.RoleId equals role.Id
                    where role.Name == "Admin"
                    orderby user.FullName
                    select user)
                    .Distinct()
                    .ToList();
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult MyTickets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = _appDbContext.tickets
            .Include(t => t.Status)
            .Include(t => t.Priority)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToList();

            return View(list);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult CreateUserTicket()
        {
            return View(new CreateUserTicketViewModel());
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserTicket(CreateUserTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if(string.IsNullOrEmpty(userId))
                {
                    return Forbid();
                }
                {
                    var ticket = new Ticket
                    {
                        Title = model.Title,
                        Description = model.Description,
                        TicketNumber = $"T-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4)}",
                        CreatedById = userId,
                        CreatedAt = DateTime.UtcNow
                    };

                    _appDbContext.tickets.Add(ticket);
                    await _appDbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(MyTickets));
                }
                return View(model);
            }
        }
    }

    class Ticket
    {
        public string TicketNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedById { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
