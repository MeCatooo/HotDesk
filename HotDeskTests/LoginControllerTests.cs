using HotDesk.Models;
using JwtApp.Controllers;
using JwtApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotDeskTests
{
    [TestClass]
    public class LoginControllerTests
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
        public void LoginTest()
        {
            var repository = GetContext();
            var controller = new LoginController(repository);
            var loginModel = new UserLogin() { Username = "Test1", Password = "123" };
            repository.AddUser(loginModel);
            var result = controller.Login(loginModel);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
        [TestMethod]
        public void LoginWrongTest()
        {
            var repository = GetContext();
            var controller = new LoginController(repository);
            var loginModel = new UserLogin() { Username = "Test2", Password = "123" };
            var result = controller.Login(loginModel);
            var found = repository.GetUser(loginModel);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
        [TestMethod]
        public void RegisterTest()
        {
            var repository = GetContext();
            var controller = new LoginController(repository);
            var loginModel = new UserLogin() { Username = "Test3", Password = "123" };
            var result = controller.Register(loginModel);
            var found = repository.GetUser(loginModel);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(found.Username, loginModel.Username);
            Assert.AreEqual(found.Password, loginModel.Password);
        }
    }
}
