using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.Controllers
{
    public class TripsController : Controller
    {
        private readonly DataContext _context;

        public TripsController(DataContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Index()
        {
            var trips = await _context.Trips
                .Include(t => t.typeOfHoliday)
                .AsNoTracking()
                .ToListAsync();

            return View(trips);
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();

            var trip = await _context.Trips
                .Include(t => t.typeOfHoliday)
                .Include(t => t.Services)
                .Include(t => t.AdditionalServices)
                .Include(t => t.Orders)
                .AsSplitQuery() 
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null) return NotFound();

            return View(trip);
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Create()
        {
            ViewBag.TypeOfHolidays = await _context.TypeOfHolidays.AsNoTracking().ToListAsync();
            return View();
        }

        [Authorize(Roles = "Admin,Moder")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trip trip)
        {
    
            ModelState.Remove("typeOfHoliday");
            ModelState.Remove("Services");
            ModelState.Remove("AdditionalServices");
            ModelState.Remove("Orders");

            if (ModelState.IsValid)
            {
                _context.Add(trip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TypeOfHolidays = await _context.TypeOfHolidays.AsNoTracking().ToListAsync();
            return View(trip);
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();

            ViewBag.TypeOfHolidays = await _context.TypeOfHolidays.AsNoTracking().ToListAsync();
            return View(trip);
        }

        [Authorize(Roles = "Admin,Moder")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Trip updatedTrip)
        {
            if (id != updatedTrip.Id) return BadRequest();

            ModelState.Remove("typeOfHoliday");
            ModelState.Remove("Services");
            ModelState.Remove("AdditionalServices");
            ModelState.Remove("Orders");

            if (ModelState.IsValid)
            {
                try
                {
                    var tripInDb = await _context.Trips.FindAsync(id);
                    if (tripInDb == null) return NotFound();

                    tripInDb.Title = updatedTrip.Title;
                    tripInDb.Country = updatedTrip.Country;
                    tripInDb.City = updatedTrip.City;
                    tripInDb.HotelName = updatedTrip.HotelName;
                    tripInDb.HotelAddress = updatedTrip.HotelAddress;
                    tripInDb.typeofHoldayId = updatedTrip.typeofHoldayId;
                    tripInDb.NumberOfNights = updatedTrip.NumberOfNights;
                    tripInDb.Price = updatedTrip.Price;
                    tripInDb.Quantity = updatedTrip.Quantity;
                    tripInDb.date = updatedTrip.date;
                    tripInDb.status = updatedTrip.status;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(updatedTrip.Id)) return NotFound();
                    else throw;
                }
            }

            ViewBag.TypeOfHolidays = await _context.TypeOfHolidays.AsNoTracking().ToListAsync();
            return View(updatedTrip);
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            var trip = await _context.Trips
                .Include(t => t.typeOfHoliday)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null) return NotFound();

            return View(trip);
        }

        [Authorize(Roles = "Admin,Moder")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripExists(long id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }
    }
}