using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.Controllers
{
    public class ServicesController : Controller
    {
        private readonly DataContext _context;

        public ServicesController(DataContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Index()
        {
  
            var services = await _context.Services
                .Include(s => s.Trips)
                .Include(s => s.TripsAdditional)
                .Include(s => s.Orders)
                .AsNoTracking()
                .ToListAsync();

            return View(services);
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services
                .Include(s => s.Trips)
                .Include(s => s.TripsAdditional)
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null) return NotFound();

            return View(service);
        }

        [Authorize(Roles = "Admin,Moder")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Moder")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TypeService,Price")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services.FindAsync(id);
            if (service == null) return NotFound();

            return View(service);
        }

        [Authorize(Roles = "Admin,Moder")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,TypeService,Price")] Service updatedService)
        {
            if (id != updatedService.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                   
                    var serviceInDb = await _context.Services.FindAsync(id);
                    if (serviceInDb == null) return NotFound();

                    serviceInDb.Name = updatedService.Name;
                    serviceInDb.TypeService = updatedService.TypeService;
                    serviceInDb.Price = updatedService.Price;

                    _context.Update(serviceInDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(updatedService.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updatedService);
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var service = await _context.Services
                .Include(s => s.Orders) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (service == null) return NotFound();

            return View(service);
        }

        [Authorize(Roles = "Admin,Moder")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var service = await _context.Services
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null) return NotFound();


            if (service.Orders != null && service.Orders.Any())
            {
                ModelState.AddModelError("", "Нельзя удалить услугу, так как она используется в заказах.");
                return View(service);
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(long id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}