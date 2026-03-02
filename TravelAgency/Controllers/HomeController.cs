using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data; 
using TravelAgency.Models;
[Authorize]
public class HomeController : Controller
{
    private readonly DataContext _context;

    public HomeController(DataContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {

        var activeTrips = await _context.Trips
            .Where(t => t.status == StatusTrip.Active)
            .OrderBy(t => t.date)
            .ToListAsync();

        return View(activeTrips);
    }
}