using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Models;

public class PassangersController : Controller
{
    private readonly DataContext _context;

    public PassangersController(DataContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        var passangers = _context.Passangers
            .Include(p => p.user)
            .Include(p => p.Orders)
            .ToList();

        return View(passangers);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Details(long id)
    {
        var passanger = _context.Passangers
            .Include(p => p.user)
            .Include(p => p.Orders)
            .FirstOrDefault(p => p.Id == id);

        if (passanger == null) return NotFound();

        return View(passanger);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewBag.Users = _context.Users.ToList(); 
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Passanger passanger)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Users = _context.Users.ToList();
            return View(passanger);
        }

        _context.Passangers.Add(passanger);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Edit(long id)
    {
        var passanger = _context.Passangers.Find(id);
        if (passanger == null) return NotFound();

        ViewBag.Users = _context.Users.ToList();
        return View(passanger);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(long id, Passanger updatedPassanger)
    {
        if (id != updatedPassanger.Id) return BadRequest();

        var passanger = _context.Passangers.Find(id);
        if (passanger == null) return NotFound();

        passanger.FirstName = updatedPassanger.FirstName;
        passanger.LastName = updatedPassanger.LastName;
        passanger.MiddleName = updatedPassanger.MiddleName;
        passanger.BirthCertificate = updatedPassanger.BirthCertificate;
        passanger.Passport = updatedPassanger.Passport;
        passanger.InternationalPassport = updatedPassanger.InternationalPassport;
        passanger.Policy = updatedPassanger.Policy;


        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Delete(long id)
    {
        var passanger = _context.Passangers.Find(id);
        if (passanger == null) return NotFound();

        return View(passanger);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(long id)
    {
        var passanger = _context.Passangers.Find(id);
        if (passanger == null) return NotFound();

        _context.Passangers.Remove(passanger);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
