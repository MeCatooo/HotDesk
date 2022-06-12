using HotDesk.Controllers;
using HotDesk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotDeskTests
{
    [TestClass]
    public class LocationControllerTests
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
            var result = controller.Create("Kraków") as OkObjectResult;
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(result.Value, repository.GetLocation((result.Value as Location).Id));
        }
        [TestMethod]
        public void AddDeskToLocationTest()
        {
            var repository = GetContext();
            var controller = new LocationsController(repository);
            controller.Create("Kraków");
            var result = controller.AddDesk(1, "NextToWindow") as OkObjectResult;
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(repository.GetDesk((result.Value as Location).Id));
        }
        [TestMethod]
        public void RemoveDeskFromLocationTest()
        {
            var repository = GetContext();
            var controller = new LocationsController(repository);
            controller.Create("Kraków");
            controller.AddDesk(1, "NextToWindow");
            var result = controller.RemoveDesk(1) as OkResult;
            var found = repository.GetDesk(1);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.IsNull(found);
        }
        [TestMethod]
        public void GetLocationTest()
        {
            var repository = GetContext();
            var controller = new LocationsController(repository);
            controller.Create("Kraków");
            var result = controller.Details(1) as OkObjectResult;
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(result.Value, repository.GetLocation((result.Value as Location).Id));
        }
        [TestMethod]
        public void GetAllLocationTest()
        {
            var repository = GetContext();
            var controller = new LocationsController(repository);
            var result = controller.Index() as OkObjectResult;
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public void GetLocationWrongTest()
        {
            var repository = GetContext();
            var controller = new LocationsController(repository);
            controller.Create("Kraków");
            var result = controller.Details(777);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public void ChangeDeskStateTest()
        {
            var repository = GetContext();
            var controller = new LocationsController(repository);
            controller.Create("Kraków");
            controller.AddDesk(1, "NextToWindow");
            var result = controller.ChangeStateDesk(1, true) as OkResult;
            var found = repository.GetDesk(1);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.AreEqual(found.Unavailable, true);
        }
        [TestMethod]
        public void RemoveLocationTest()
        {
            var repository = GetContext();
            var location = repository.AddLocation(new Location() { Name = "ToDelete" });
            var controller = new LocationsController(repository);
            var result = controller.RemoveLocation(location.Id);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.IsNull(repository.GetLocation(location.Id));
        }
        [TestMethod]
        public void RemoveNotEmptyLocationTest()
        {
            var repository = GetContext();
            var location = repository.AddLocation(new Location() { Name = "ToDelete" });
            var desk = repository.AddDesk(new Desk() { Name = "1", Location = location });
            var controller = new LocationsController(repository);
            var result = controller.RemoveLocation(location.Id);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.IsNotNull(repository.GetLocation(location.Id));
        }
    }
}