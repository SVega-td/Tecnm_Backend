using HelpDeskWeb.Data;
using HelpDeskWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskWeb.Controllers
{
    public class TicketStatusController : Controller
    {
        private AppDbContext _appDbContext;

        public TicketStatusController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<TicketStatus> ticketStatusList = _appDbContext.TicketStatus.ToList();
            return View(ticketStatusList);
        }

        // 1. Mostrar formulario de creación
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 2. Guardar el nuevo registro
        [HttpPost]
        public async Task<IActionResult> Create(TicketStatus ticketStatus)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.TicketStatus.Add(ticketStatus);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketStatus);
        }

        // 3. Mostrar formulario de edición
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ticketStatus = _appDbContext.TicketStatus.Find(id);
            if (ticketStatus == null)
            {
                return NotFound();
            }
            return View(ticketStatus);
        }

        // 4. Guardar los cambios de la edición
        [HttpPost]
        public async Task<IActionResult> Edit(TicketStatus ticketStatus)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.TicketStatus.Update(ticketStatus);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketStatus);
        }

        // 5. Eliminar un registro
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var ticketStatus = _appDbContext.TicketStatus.Find(id);
            if (ticketStatus == null)
            {
                return NotFound();
            }
            _appDbContext.TicketStatus.Remove(ticketStatus);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}