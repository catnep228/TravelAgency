using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using TravelAgency.Data;
using TravelAgency.Models;

namespace Test
{
    [TestFixture]
    public class TestOrder
    {
        private DataContext _context;
        private OrdersController _controller;

        [SetUp]
        public void Setup()
        {
            var mockConfig = new Mock<IConfiguration>();
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options, mockConfig.Object);
            _controller = new OrdersController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void Index_ReturnsViewResult_WithListOfOrders()
        {
            var holidayType = new TypeOfHoliday { Id = 1, Name = "Beach", TypeOfACt = TypeActivity.Chill };
            _context.TypeOfHolidays.Add(holidayType);

            var trip = new Trip
            {
                Id = 1,
                Title = "Test Trip",
                Country = "Turkey",
                City = "Antalya",
                HotelName = "Sun Hotel",
                HotelAddress = "Main Str 1",
                typeofHoldayId = 1,
                typeOfHoliday = holidayType,
                date = DateTime.Now
            };
            _context.Trips.Add(trip);
            _context.SaveChanges();

            var order = new Order
            {
                Id = 1,
                Date = DateTime.Now,
                TripId = 1,
                Trip = trip,
                Status = StatusOrder.Active,
                Passangers = new List<Passanger>(),
                AdditionalServices = new List<Service>()
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            var result = _controller.Index() as ViewResult;

            Assert.That(result, Is.Not.Null);
            var model = result.Model as List<Order>;
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddOrder_Post_ValidModel_RedirectsToMyOrders()
        {
            var userEmail = "test@user.com";

            var holidayType = new TypeOfHoliday { Id = 1, Name = "Beach", TypeOfACt = TypeActivity.Chill };
            _context.TypeOfHolidays.Add(holidayType);

            var trip = new Trip
            {
                Id = 1,
                Title = "Dest",
                Country = "Turkey",
                City = "Antalya",
                HotelName = "Sun Hotel",
                HotelAddress = "Main Str 1",
                typeofHoldayId = 1,
                typeOfHoliday = holidayType,
                date = DateTime.Now
            };
            _context.Trips.Add(trip);

            var passenger = new Passanger { Id = 1, FirstName = "Ivan" };
            var user = new User
            {
                Id = 1,
                Email = userEmail,
                Password = "hashed_password",
                PhoneNumber = "123456789",
                Passanger = passenger
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, userEmail)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            var newOrder = new Order
            {
                Id = 10,
                Date = DateTime.Now,
                TripId = 1,
                Trip = trip,
                Passangers = new List<Passanger>()
            };

            var result = _controller.AddOrder(newOrder, new long[] { }) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo("MyOrders"));

            var savedOrder = _context.Orders
                .Include(o => o.Passangers)
                .ThenInclude(p => p.user)
                .FirstOrDefault(o => o.Id == 10);

            Assert.That(savedOrder, Is.Not.Null);
            Assert.That(savedOrder.Passangers.Count, Is.EqualTo(1));
            Assert.That(savedOrder.Passangers.First().user.Email, Is.EqualTo(userEmail));
        }
    }
}
