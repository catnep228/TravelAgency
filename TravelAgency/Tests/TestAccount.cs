using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using TravelAgency.Controllers;
using TravelAgency.Data;
using TravelAgency.Models;

namespace TravelAgency.Tests
{
    [TestFixture]
    public class TestAccount
    {
        private DataContext _context;
        private AccountController _controller;
        private Mock<IAuthenticationService> _authMock;

        [SetUp]
        public void Setup()
        {
            var configMock = new Mock<IConfiguration>();

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            _context = new DataContext(options, configMock.Object);

            _authMock = new Mock<IAuthenticationService>();
            _authMock
                .Setup(x => x.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            var tempDataMock = new Mock<ITempDataProvider>();

            var services = new ServiceCollection()
                .AddLogging()
                .AddSingleton(_authMock.Object)
                .AddSingleton(tempDataMock.Object)
                .BuildServiceProvider();

            var httpContext = new DefaultHttpContext
            {
                RequestServices = services
            };

            _controller = new AccountController(_context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                },
                TempData = new TempDataDictionary(httpContext, tempDataMock.Object)
            };
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        

        [Test]
        public async Task Login_InvalidCredentials_ReturnsView()
        {
            var user = new User
            {
                Email = "real@test.com",
                Password = "correct",
                PhoneNumber = "123",
                RoleId = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _controller.Login("real@test.com", "wrong");

            var view = result as ViewResult;

            Assert.That(view, Is.Not.Null);
            Assert.That(_controller.ModelState.IsValid, Is.False);

            _authMock.Verify(x => x.SignInAsync(
                It.IsAny<HttpContext>(),
                It.IsAny<string>(),
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<AuthenticationProperties>()),
                Times.Never);
        }
    }
}
