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
        public void GetAllReservations()
        {
            var repository = GetContext();
            var controller = new ReservationsController(repository);
            var result = controller.Index() as OkObjectResult;
            Assert.IsNotNull(result.Value);
        }

        //[TestMethod]
        //public void AddReservationFree()
        //{
        //    var repository = GetContext();
        //    repository.AddUser(new JwtApp.Models.UserLogin() { Username = "login", Password = "123" });

        //    var controllerLocations = new LocationsController(repository);
        //    var controller = new ReservationsController(repository);
        //    var controllerLogin = new LoginController(repository);
        //    controllerLocations.Create("Warszawa");
        //    controllerLocations.AddDesk(1,"NextToWindow");
        //    controller.ControllerContext = new ControllerContext();
        //    controller.ControllerContext.HttpContext = new DefaultHttpContext();
        //    controller.ControllerContext.HttpContext.Request.Headers.Authorization = "Bearer "+controllerLogin.Login(new UserLogin() { Username = "login", Password = "123" });
        //    var result = controller.Create(1, 1, new TimeStamps() { From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 3) }) as OkObjectResult;
        //    Assert.IsTrue((result.Value as Reservation).desk.Name== "NextToWindow");
        //}
    }
}
