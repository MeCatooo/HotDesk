using HotDesk.Models;
using JwtApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotDeskTests
{
    [TestClass]
    public class ReservationLogicTests
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
        public void IsDeskReserved_Free_Test()
        {
            var repository = GetContext();
            var location = repository.AddLocation(new Location() { Name = "Kraków" });
            var desk = repository.AddDesk(new Desk() { Name = "1", Location = location });
            var desk1 = repository.AddDesk(new Desk() { Name = "1", Location = location });
            var user = repository.AddUser(new UserLogin() { Username = "Jan", Password = "Kowalski" });
            var result = ReservationLogic.IsDeskReserved(desk, new DateTime(2023, 1, 1), new DateTime(2023, 1, 2), repository);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void IsDeskReserved_Occupied_Test()
        {
            var repository = GetContext();
            var location = repository.AddLocation(new Location() { Name = "Kraków" });
            var desk = repository.AddDesk(new Desk() { Name = "1", Location = location });
            var desk1 = repository.AddDesk(new Desk() { Name = "1", Location = location });
            var user = repository.AddUser(new UserLogin() { Username = "Stefan", Password = "Kowalski" });
            var reservation = repository.AddReservation(new Reservation() { From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 4), Desk = desk, Location = location, User = user });
            var result = ReservationLogic.IsDeskReserved(desk, new DateTime(2023, 1, 3), new DateTime(2023, 1, 5), repository);
            Assert.IsFalse(result);
            result = ReservationLogic.IsDeskReserved(desk, new DateTime(2022, 12, 30), new DateTime(2023, 1, 2), repository);
            Assert.IsFalse(result);
            result = ReservationLogic.IsDeskReserved(desk, new DateTime(2022, 12, 30), new DateTime(2023, 1, 5), repository);
            Assert.IsFalse(result);
            result = ReservationLogic.IsDeskReserved(desk, new DateTime(2023, 1, 1), new DateTime(2023, 1, 2), repository);
            Assert.IsFalse(result);
        }
        [TestMethod]
        public void FreeDesksTest()
        {
            var repository = GetContext();
            var location = repository.AddLocation(new Location() { Name = "Kraków" });
            var desk = repository.AddDesk(new Desk() { Name = "1", Location = location });
            var desk1 = repository.AddDesk(new Desk() { Name = "1", Location = location });
            var user = repository.AddUser(new UserLogin() { Username = "Alex1", Password = "Kowalski" });
            var reservation = repository.AddReservation(new Reservation() { From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 4), Desk = desk, Location = location, User = user });
            var result = ReservationLogic.FreeDesks(location.Id ,new DateTime(2023, 1, 1), new DateTime(2023, 1, 2), repository);
            Assert.IsFalse(result.Any(a=>a.Id==desk.Id));
            Assert.IsTrue(result.Any(a=>a.Id==desk1.Id));
        }
        [TestMethod]
        public void OccupiedDesksTest()
        {
            var repository = GetContext();
            var location = repository.AddLocation(new Location() { Name = "Kraków" });
            var desk = repository.AddDesk(new Desk() { Name = "1", Location = location });
            var desk1 = repository.AddDesk(new Desk() { Name = "1", Location = location });
            var user = repository.AddUser(new UserLogin() { Username = "Kevin", Password = "Kowalski" });
            var reservation = repository.AddReservation(new Reservation() { From = new DateTime(2023, 1, 1), To = new DateTime(2023, 1, 4), Desk = desk, Location = location, User = user });
            var result = ReservationLogic.OccupiedDesks(location.Id ,new DateTime(2023, 1, 1), new DateTime(2023, 1, 2), repository);
            Assert.IsFalse(result.Any(a=>a.Id==desk1.Id));
            Assert.IsTrue(result.Any(a=>a.Id==desk.Id));
        }
    }
}
