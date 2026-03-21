using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {

            ModelState.Remove("Role");
            ModelState.Remove("PassangerId");
            ModelState.Remove("Passanger.user");
            ModelState.Remove("Passanger.Orders");

            if (!ModelState.IsValid)
            {

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return View(user);
            }

            bool userExists = await _context.Users.AnyAsync(u => u.Email == user.Email);
            if (userExists)
            {
                ModelState.AddModelError("Email", "Этот Email уже занят.");
                return View(user);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Client")
                                  ?? await _context.Roles.FirstOrDefaultAsync();

                user.RoleId = defaultRole?.Id ?? 1;


                if (user.Passanger != null)
                {
                    _context.Passangers.Add(user.Passanger);
                    await _context.SaveChangesAsync(); 

                    user.PassangerId = user.Passanger.Id;
                }


                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, defaultRole?.Name ?? "User")
        };

                var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
                await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                ModelState.AddModelError("", "Ошибка БД: " + ex.InnerException?.Message ?? ex.Message);
                return View(user);
            }
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.Users
                .Include(u => u.Passanger)
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role?.Name ?? "User")
    };

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Index", "Home");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
