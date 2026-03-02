using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.Controllers
{
    public class TypeOfHolidayController : Controller
    {
        private readonly DataContext _context;

        public TypeOfHolidayController(DataContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Index()
        {
            var holidays = await _context.TypeOfHolidays.Include(th => th.Trips).ToListAsync();
            return View(holidays);
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var holiday = await _context.TypeOfHolidays
                .Include(th => th.Trips)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (holiday == null) return NotFound();

            return View(holiday);
        }

        [Authorize(Roles = "Admin,Moder")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Moder")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,TypeOfACt")] TypeOfHoliday typeOfHoliday)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeOfHoliday);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfHoliday);
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var typeOfHoliday = await _context.TypeOfHolidays.FindAsync(id);
            if (typeOfHoliday == null) return NotFound();

            return View(typeOfHoliday);
        }

        [Authorize(Roles = "Admin,Moder")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,TypeOfACt")] TypeOfHoliday typeOfHoliday)
        {
            if (id != typeOfHoliday.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeOfHoliday);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfHolidayExists(typeOfHoliday.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfHoliday);
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var typeOfHoliday = await _context.TypeOfHolidays
                .Include(th => th.Trips)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (typeOfHoliday == null) return NotFound();

            return View(typeOfHoliday);
        }

        [Authorize(Roles = "Admin,Moder")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var typeOfHoliday = await _context.TypeOfHolidays.FindAsync(id);
            if (typeOfHoliday != null)
            {
                _context.TypeOfHolidays.Remove(typeOfHoliday);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TypeOfHolidayExists(long id)
        {
            return _context.TypeOfHolidays.Any(e => e.Id == id);
        }
    }
}
