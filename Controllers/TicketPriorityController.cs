using HelpDeskWeb.Data;
using HelpDeskWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskWeb.Controllers
{
    public class TicketPriorityController : Controller
    {
        private AppDbContext _appDbContext;

        public TicketPriorityController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<TicketPriority> ticketPriorityList = _appDbContext.TicketPriorities.ToList();
            return View(ticketPriorityList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketPriority ticketPriority)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.TicketPriorities.Add(ticketPriority);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketPriority);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ticketPriority = _appDbContext.TicketPriorities.Find(id);
            if (ticketPriority == null)
            {
                return NotFound();
            }
            return View(ticketPriority);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TicketPriority ticketPriority)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.TicketPriorities.Update(ticketPriority);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketPriority);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var ticketPriority = _appDbContext.TicketPriorities.Find(id);
            if (ticketPriority == null)
            {
                return NotFound();
            }
            _appDbContext.TicketPriorities.Remove(ticketPriority);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}