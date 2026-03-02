using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TravelAgency.Data;
using TravelAgency.Models;


namespace TravelAgency.Controllers
{
 
    public class UsersController : Controller
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.roleId = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.roleId = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name", user.RoleId);
                return View(user);
            }

            if (user.Passanger != null)
            {
                _context.Passangers.Add(user.Passanger);
                await _context.SaveChangesAsync();

                user.PassangerId = user.Passanger.Id;
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Include(u => u.Passanger)
                .Include(u => u.Role)
                .ToListAsync();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(long id)
        {
            var user = await _context.Users
                .Include(u => u.Passanger)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();


            ViewData["RoleList"] = new SelectList(_context.Roles, "Id", "Name", user.RoleId);
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, User updatedUser)
        {
            if (id != updatedUser.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewData["RoleList"] = new SelectList(_context.Roles, "Id", "Name", updatedUser.RoleId);
                return View(updatedUser);
            }


            var user = await _context.Users
                .Include(u => u.Passanger)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();


            user.Email = updatedUser.Email;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.RoleId = updatedUser.RoleId;

            if (!string.IsNullOrWhiteSpace(updatedUser.Password))
            {
                user.Password = updatedUser.Password;
            }


            if (user.Passanger != null && updatedUser.Passanger != null)
            {
                user.Passanger.FirstName = updatedUser.Passanger.FirstName;
                user.Passanger.LastName = updatedUser.Passanger.LastName;
                user.Passanger.MiddleName = updatedUser.Passanger.MiddleName;
                user.Passanger.Passport = updatedUser.Passanger.Passport;
                user.Passanger.InternationalPassport = updatedUser.Passanger.InternationalPassport;
                user.Passanger.Policy = updatedUser.Passanger.Policy;
                user.Passanger.BirthCertificate = updatedUser.Passanger.BirthCertificate;
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
                ViewData["RoleList"] = new SelectList(_context.Roles, "Id", "Name", updatedUser.RoleId);
                return View(updatedUser);
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long id)
        {
            var user = await _context.Users
                .Include(u => u.Passanger)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

  
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Passanger)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var user = await _context.Users
                .Include(u => u.Passanger)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound();

            if (user.Passanger != null)
                _context.Passangers.Remove(user.Passanger);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
