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
        public void GetReservationTest()
        {
            var repository = GetContext();
            var controller = new ReservationsController(repository);
            var controllerLogin = new LoginController(repository);
            var location = repository.AddLocation(new Location() { Name = "Kraków" });
            var desk = repository.AddDesk(new Desk() { Name = "1", Location = location });
            var user = repository.AddUser(new UserLogin() { Username = "Alex", Password = "Kowalski" });
            var reservation = repository.AddReservation(new Reservation() { From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 4), Desk = desk, Location = location, User = user });

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers.Authorization = "Bearer " + controllerLogin.Login(new UserLogin() { Username = "Alex", Password = "Kowalski" });

            var result = controller.Details(1);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull((result as OkObjectResult).Value);
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
        public void GetAllReservationsAdminTest()
        {
            var repository = GetContext();
            var controller = new ReservationsController(repository);
            var controllerLogin = new LoginController(repository);
            var user = repository.AddUser(new UserLogin() { Username = "AdminTest", Password = "Kowalski" });
            repository.UpdateUser("AdminTest", "Administrator");
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers.Authorization = "Bearer " + controllerLogin.Login(new UserLogin() { Username = "AdminTest", Password = "Kowalski" });

            var result = controller.Index() as OkObjectResult;


            Assert.IsNotNull((result.Value as IEnumerable<Reservation>).All(a => ReferenceEquals(a.Desk, null)));
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
            Assert.IsTrue((result.Value as Reservation).Desk.Name == "NextToWindow");
        }
        [TestMethod]
        public void AddReservation_OccupiedTest()
        {
            var repository = GetContext();
            var user = repository.AddUser(new UserLogin() { Username = "Fernando", Password = "123" });
            var controller = new ReservationsController(repository);
            var controllerLogin = new LoginController(repository);

            var location = repository.AddLocation(new Location() { Name = "Kraków" });
            var desk = repository.AddDesk(new Desk() { Name = "NextToWindow", Location = location });
            var reservation = repository.AddReservation(new Reservation() { From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 4), Desk = desk, Location = location, User = user });
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers.Authorization = "Bearer " + controllerLogin.Login(new UserLogin() { Username = user.Username, Password = user.Password });
            var result = controller.Create(location.Id, desk.Id, new TimeStamps() { From = new DateTime(2023, 1, 2), To = new DateTime(2023, 1, 3) });
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            result = controller.Create(location.Id, desk.Id, new TimeStamps() { From = new DateTime(2022, 12, 30), To = new DateTime(2023, 1, 5) });
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            result = controller.Create(location.Id, desk.Id, new TimeStamps() { From = new DateTime(2023, 1, 3), To = new DateTime(2023, 1, 6) });
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            result = controller.Create(location.Id, desk.Id, new TimeStamps() { From = new DateTime(2022, 12, 30), To = new DateTime(2023, 1, 6) });
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
            var reservation = repository.AddReservation(new Reservation() { From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 4), Desk = desk, Location = location, User = user });
            var result = controller.Edit(reservation.Id, desk1.Id) as OkObjectResult;
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsTrue((result.Value as Reservation).Desk.Name == "NextToWindow1");
        }
    }
}
