using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.Controllers
{
    public class TypeOfHolidaysController : Controller
    {
        private readonly DataContext _context;

        public TypeOfHolidaysController(DataContext context)
        {
            _context = context;
        }

        // GET: TypeOfHolidays
        public async Task<IActionResult> Index()
        {
            return View(await _context.TypeOfHolidays.ToListAsync());
        }

        // GET: TypeOfHolidays/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfHoliday = await _context.TypeOfHolidays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeOfHoliday == null)
            {
                return NotFound();
            }

            return View(typeOfHoliday);
        }

        // GET: TypeOfHolidays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeOfHolidays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TypeOfACt")] TypeOfHoliday typeOfHoliday)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeOfHoliday);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfHoliday);
        }

        // GET: TypeOfHolidays/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfHoliday = await _context.TypeOfHolidays.FindAsync(id);
            if (typeOfHoliday == null)
            {
                return NotFound();
            }
            return View(typeOfHoliday);
        }

        // POST: TypeOfHolidays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,TypeOfACt")] TypeOfHoliday typeOfHoliday)
        {
            if (id != typeOfHoliday.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeOfHoliday);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfHolidayExists(typeOfHoliday.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfHoliday);
        }

        // GET: TypeOfHolidays/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfHoliday = await _context.TypeOfHolidays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeOfHoliday == null)
            {
                return NotFound();
            }

            return View(typeOfHoliday);
        }

        // POST: TypeOfHolidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var typeOfHoliday = await _context.TypeOfHolidays.FindAsync(id);
            if (typeOfHoliday != null)
            {
                _context.TypeOfHolidays.Remove(typeOfHoliday);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeOfHolidayExists(long id)
        {
            return _context.TypeOfHolidays.Any(e => e.Id == id);
        }
    }
}
