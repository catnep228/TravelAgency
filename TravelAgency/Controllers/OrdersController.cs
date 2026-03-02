using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelAgency.Data;
using TravelAgency.Models;
[Authorize]
public class OrdersController : Controller
{
    private readonly DataContext _context;

    public OrdersController(DataContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,Moder")]
    public IActionResult Index()
    {
        var orders = _context.Orders
            .Include(o => o.Passangers)
            .Include(o => o.AdditionalServices)
            .Include(o => o.Trip)
            .ToList();

        return View(orders);
    }

    [Authorize(Roles = "Admin,Moder")]
    public IActionResult Details(long id)
    {
        var order = _context.Orders
            .Include(o => o.Passangers)
            .Include(o => o.AdditionalServices)
            .Include(o => o.Trip)
            .FirstOrDefault(o => o.Id == id);

        if (order == null) return NotFound();

        return View(order);
    }

    [Authorize(Roles = "Admin,Moder")]
    public IActionResult Create()
    {
        ViewBag.Trips = _context.Trips.ToList();
        ViewBag.Passangers = _context.Passangers.ToList();
        ViewBag.Services = _context.Services.ToList();
        return View();
    }

    [Authorize(Roles = "Admin,Moder")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Order order, long[] selectedServices)
    {

        ModelState.Remove("Trip");
        foreach (var p in order.Passangers)
        {
            ModelState.Remove($"Passangers[{order.Passangers.IndexOf(p)}].Orders");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Trips = _context.Trips.ToList();
            ViewBag.Services = _context.Services.ToList();
            return View(order);
        }


        if (selectedServices != null && selectedServices.Length > 0)
        {
            order.AdditionalServices = _context.Services
                .Where(s => selectedServices.Contains(s.Id))
                .ToList();
        }

        try
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {

            ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
            ViewBag.Trips = _context.Trips.ToList();
            ViewBag.Services = _context.Services.ToList();
            return View(order);
        }
    }
    [Authorize(Roles = "Admin,Moder")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(long id, Order updatedOrder, long[] selectedServices)
    {
        if (id != updatedOrder.Id) return BadRequest();


        var dbOrder = _context.Orders
            .Include(o => o.Passangers)
            .Include(o => o.AdditionalServices)
            .FirstOrDefault(o => o.Id == id);

        if (dbOrder == null) return NotFound();


        dbOrder.Date = updatedOrder.Date;
        dbOrder.Status = updatedOrder.Status;
        dbOrder.TripId = updatedOrder.TripId;

        var updatedPassengerIds = updatedOrder.Passangers.Select(p => p.Id).ToList();
        var passengersToRemove = dbOrder.Passangers
            .Where(p => !updatedPassengerIds.Contains(p.Id)).ToList();

        foreach (var p in passengersToRemove)
        {
            dbOrder.Passangers.Remove(p);
        }


        foreach (var p in updatedOrder.Passangers)
        {
            var existingPassenger = dbOrder.Passangers.FirstOrDefault(dbP => dbP.Id == p.Id && p.Id != 0);

            if (existingPassenger != null)
            {

                _context.Entry(existingPassenger).CurrentValues.SetValues(p);
            }
            else
            {

                dbOrder.Passangers.Add(p);
            }
        }


        dbOrder.AdditionalServices.Clear();
        if (selectedServices != null)
        {
            dbOrder.AdditionalServices = _context.Services
                .Where(s => selectedServices.Contains(s.Id))
                .ToList();
        }

        try
        {
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
            ViewBag.Trips = _context.Trips.ToList();
            ViewBag.Services = _context.Services.ToList();
            return View(updatedOrder);
        }
    }
    [Authorize(Roles = "Admin,Moder")]
    public IActionResult Edit(long id)
    {
        var order = _context.Orders
            .Include(o => o.Passangers)
            .Include(o => o.AdditionalServices)
            .FirstOrDefault(o => o.Id == id);

        if (order == null) return NotFound();

        ViewBag.Trips = _context.Trips.ToList();
        ViewBag.Passangers = _context.Passangers.ToList();
        ViewBag.Services = _context.Services.ToList();

        return View(order);
    }


    [Authorize(Roles = "Admin,Moder")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(long id)
    {
        var order = _context.Orders.Find(id);
        if (order == null) return NotFound();

        _context.Orders.Remove(order);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult AddOrder(long? tripId)
    {
        ViewBag.Trips = _context.Trips.ToList();
        ViewBag.Services = _context.Services.ToList();

        var order = new Order
        {
            Date = DateTime.Now,
            Status = StatusOrder.Active, 
            TripId = tripId ?? 0        
        };

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddOrder(Order order, long[] selectedServices)
    {

        ModelState.Remove("Trip");

        if (!ModelState.IsValid)
        {
            ViewBag.Trips = _context.Trips.ToList();
            ViewBag.Services = _context.Services.ToList();
            return View("AddOrder", order);
        }


        var userEmail = User.Identity.Name;
        var currentUser = _context.Users
            .Include(u => u.Passanger) 
            .FirstOrDefault(u => u.Email == userEmail);

        if (currentUser != null)
        {

            if (currentUser.Passanger != null)
            {

                order.Passangers.Add(currentUser.Passanger);
            }
        }


        if (selectedServices != null)
        {
            order.AdditionalServices = _context.Services
                .Where(s => selectedServices.Contains(s.Id)).ToList();
        }


        _context.Orders.Add(order);
        _context.SaveChanges();

        return RedirectToAction("MyOrders");
    }

    [Authorize] 
    public IActionResult MyOrders()
    {
   
        var userEmail = User.Identity.Name;

        var orders = _context.Orders
            .Include(o => o.Trip)
            .Include(o => o.AdditionalServices)
            .Include(o => o.Passangers)
                .ThenInclude(p => p.user)
            .Where(o => o.Passangers.Any(p => p.user.Email == userEmail))
            .OrderByDescending(o => o.Date)
            .ToList();

        return View(orders);
    }
}
