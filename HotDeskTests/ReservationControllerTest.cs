using HotDesk.Controllers;
using HotDesk.Models;
using JwtApp.Controllers;
using JwtApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotDeskTests
{
    [TestClass]
    public class ReservationControllerTest
    {
        private HotDeskRepository GetContext()
        {
            var options = new DbContextOptionsBuilder<HotDeskDbContext>().UseInMemoryDatabase("HotDesk")
                .Options;
            var databaseContext = new HotDeskDbContext(options);
            databaseContext.Database.EnsureCreated();
            var repository = new HotDeskRepository(databaseContext);
            return repository;
        }
        [TestMethod]
        public void GetAllReservationsTest()
        {
            var repository = GetContext();
            var controller = new ReservationsController(repository);
            var result = controller.Index() as OkObjectResult;
            Assert.IsNotNull(result.Value);
        }

        [TestMethod]
        public void AddReservationFreeTest()
        {
            var repository = GetContext();
            repository.AddUser(new UserLogin() { Username = "login", Password = "123" });
            var controller = new ReservationsController(repository);
            var controllerLogin = new LoginController(repository);

            var location = repository.AddLocation(new Location() { Name = "Kraków" });
            var desk = repository.AddDesk(new Desk() { Name = "NextToWindow", Location = location });
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers.Authorization = "Bearer " + controllerLogin.Login(new UserLogin() { Username = "login", Password = "123" });
            var result = controller.Create(location.Id, desk.Id, new TimeStamps() { From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 3) }) as OkObjectResult;
            Assert.IsTrue((result.Value as Reservation).desk.Name == "NextToWindow");
        }
        [TestMethod]
        public void AddReservationFree_OccupiedTest()
        {
            var repository = GetContext();
            var user = repository.AddUser(new UserLogin() { Username = "Fernando", Password = "123" });
            var controller = new ReservationsController(repository);
            var controllerLogin = new LoginController(repository);

            var location = repository.AddLocation(new Location() { Name = "Kraków" });
            var desk = repository.AddDesk(new Desk() { Name = "NextToWindow", Location = location });
            var reservation = repository.AddReservation(new Reservation() { From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 4), desk = desk, location = location, user = user });
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers.Authorization = "Bearer " + controllerLogin.Login(new UserLogin() { Username = user.Username, Password = user.Password });
            var result = controller.Create(location.Id, desk.Id, new TimeStamps() { From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 3) });
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
        [TestMethod]
        public void ChangeDeskTest()
        {
            var repository = GetContext();
            var user = repository.AddUser(new UserLogin() { Username = "Alberto", Password = "123" });
            var controller = new ReservationsController(repository);

            var location = repository.AddLocation(new Location() { Name = "Kraków" });
            var desk = repository.AddDesk(new Desk() { Name = "NextToWindow", Location = location });
            var desk1 = repository.AddDesk(new Desk() { Name = "NextToWindow1", Location = location });
            var reservation = repository.AddReservation(new Reservation() { From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 4), desk = desk, location = location, user = user });
            var result = controller.Edit(reservation.Id, desk1.Id) as OkObjectResult;
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsTrue((result.Value as Reservation).desk.Name == "NextToWindow1");
        }
    }
}
