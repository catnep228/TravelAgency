using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Models;

public class RolesController : Controller
{
    private readonly DataContext _context;

    public RolesController(DataContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        var roles = _context.Roles
            .Include(r => r.Users)
            .ToList();

        return View(roles);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Details(long id)
    {
        var role = _context.Roles
            .Include(r => r.Users)
            .FirstOrDefault(r => r.Id == id);

        if (role == null) return NotFound();

        return View(role);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Role role)
    {
        if (!ModelState.IsValid) return View(role);

        _context.Roles.Add(role);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Edit(long id)
    {
        var role = _context.Roles.Find(id);
        if (role == null) return NotFound();

        return View(role);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(long id, Role updatedRole)
    {
        if (id != updatedRole.Id) return BadRequest();

        var role = _context.Roles.Find(id);
        if (role == null) return NotFound();

        role.Name = updatedRole.Name;
        role.Description = updatedRole.Description;

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Delete(long id)
    {
        var role = _context.Roles.Find(id);
        if (role == null) return NotFound();

        return View(role);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(long id)
    {
        var role = _context.Roles.Find(id);
        if (role == null) return NotFound();

        _context.Roles.Remove(role);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
