using HotDesk.Controllers;
using HotDesk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HotDeskTests
{
    [TestClass]
    public class UnitTest1
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
        public void AddLocationTest()
        {
            var repository = GetContext();
            var controller = new LocationsController(repository);
            var result = controller.Create("Kraków");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual((result as OkObjectResult).Value, repository.GetLocation(1));
        }
        [TestMethod]
        public void AddDeskToLocation()
        {
            var repository = GetContext();
            var controller = new LocationsController(repository);
            controller.Create("Kraków");
            var result = controller.AddDesk(1, "NextToWindow");
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(repository.GetDesk(1));
        }
    }
}