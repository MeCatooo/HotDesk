using JwtApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotDesk.Models
{
    public class HotDeskRepository : IHotDeskRepository
    {
        private HotDeskDbContext dbContext;
        public HotDeskRepository(HotDeskDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<Reservation> GetAllReservations()
        {
            return dbContext.Reservations.Include(b => b.desk).Include(b => b.location);
        }
        public IEnumerable<Reservation> GetAllReservationsAdmin()
        {
            return dbContext.Reservations.Include(b => b.desk).Include(b => b.location).Include(a => a.user);
        }
        public Reservation GetReservation(int id)
        {
            return dbContext.Reservations.Include(b => b.desk).Include(b => b.location).FirstOrDefault(a => a.Id == id);
        }
        public Reservation GetReservationAdmin(int id)
        {
            return dbContext.Reservations.Include(b => b.desk).Include(b => b.location).Include(a => a.user).FirstOrDefault(a => a.Id == id);
        }
        public Reservation AddReservation(Reservation reservation)
        {
            var _reservation = dbContext.Reservations.Add(reservation);
            dbContext.SaveChanges();
            return _reservation.Entity;
        }

        public IEnumerable<Desk> GetAllDesks()
        {
            return dbContext.Desks;
        }

        public IEnumerable<Location> GetAllLocations()
        {
            return dbContext.Locations.Include(a => a.Desks);
        }
        public Desk GetDesk(int id)
        {
            return dbContext.Desks.Find(id);
        }

        public Location GetLocation(int id)
        {
            return dbContext.Locations.Include(b => b.Desks).FirstOrDefault(a => a.Id == id);
        }

        public Desk AddDesk(Desk desk)
        {
            var _desk = dbContext.Desks.Add(desk);
            dbContext.SaveChanges();
            return _desk.Entity;
        }

        public Location AddLocation(Location location)
        {
            var _location = dbContext.Locations.Add(location);
            dbContext.SaveChanges();
            return _location.Entity;
        }

        public void RemoveDesk(int id)
        {
            dbContext.Desks.Remove(dbContext.Desks.FirstOrDefault(a => a.Id == id));
            dbContext.SaveChanges();
        }

        public void RemoveLocation(int id)
        {
            dbContext.Locations.Remove(dbContext.Locations.FirstOrDefault(a => a.Id == id));
            dbContext.SaveChanges();
        }

        public void RemoveReservation(int id)
        {
            dbContext.Reservations.Remove(dbContext.Reservations.FirstOrDefault(a => a.Id == id));
            dbContext.SaveChanges();
        }

        public Desk UpdateDeskName(int id, string name)
        {
            var deskFound = dbContext.Desks.Find(id);
            if (ReferenceEquals(deskFound, null))
                throw new ArgumentException();
            deskFound.Name = name;
            dbContext.SaveChanges();
            return deskFound;
        }

        public Location UpdateLocationName(int id, string name)
        {
            var locationFound = dbContext.Locations.Find(id);
            if (ReferenceEquals(locationFound, null))
                throw new ArgumentException();
            locationFound.Name = name;
            dbContext.SaveChanges();
            return locationFound;
        }

        public Reservation UpdateReservationDesk(int id, int deskId)
        {
            var reservationFound = dbContext.Reservations.Find(id);
            var deskFound = dbContext.Desks.Find(deskId);
            if (ReferenceEquals(reservationFound, null) || ReferenceEquals(deskFound, null))
                throw new ArgumentException();
            reservationFound.desk = deskFound;
            dbContext.SaveChanges();
            return reservationFound;
        }

        public UserModel? GetUser(UserLogin userLogin)
        {
            return dbContext.users.FirstOrDefault(o => o.Username.ToLower() == userLogin.Username.ToLower());
        }

        public UserModel AddUser(UserLogin user)
        {
            if (dbContext.users.Any(a => a.Username == user.Username))
            {
                throw new Exception("User already exists");
            }
            var userCreated = new UserModel() { Username = user.Username, Password = user.Password, Role = "User" };
            dbContext.users.Add(userCreated);
            dbContext.SaveChanges();
            return userCreated;
        }

        public UserModel UpdateUser(string username, string role)
        {
            var user = dbContext.users.FirstOrDefault(a => a.Username == username);
            user.Role = role;
            dbContext.SaveChanges();
            return user;
        }

        public Location BindDeskToLocation(int deskId, int locationId)
        {
            var desk = dbContext.Desks.FirstOrDefault(a => a.Id == deskId);
            var location = dbContext.Locations.FirstOrDefault(a => a.Id == locationId);
            location.Desks.Add(desk);
            dbContext.SaveChanges();
            return location;
        }

        public Desk UpdateDeskAvaibality(int id, bool state)
        {
            var desk = dbContext.Desks.Find(id);
            if (ReferenceEquals(desk, null))
                throw new ArgumentException();
            desk.Unavailable = state;
            dbContext.SaveChanges();
            return desk;
        }
    }
    public interface IHotDeskRepository
    {
        public IEnumerable<Reservation> GetAllReservations();
        public IEnumerable<Reservation> GetAllReservationsAdmin();
        public IEnumerable<Desk> GetAllDesks();
        public IEnumerable<Location> GetAllLocations();
        public Reservation GetReservation(int id);
        public Reservation GetReservationAdmin(int id);
        public Desk GetDesk(int id);
        public Location GetLocation(int id);
        public UserModel GetUser(UserLogin userLogin);
        public Reservation AddReservation(Reservation reservation);
        public Desk AddDesk(Desk desk);
        public Location AddLocation(Location location);
        public UserModel AddUser(UserLogin user);
        public void RemoveDesk(int id);
        public void RemoveLocation(int id);
        public void RemoveReservation(int id);
        public Desk UpdateDeskName(int id, string name);
        public Desk UpdateDeskAvaibality(int id, bool state);
        public Location UpdateLocationName(int id, string name);
        public Reservation UpdateReservationDesk(int id, int deskId);
        public UserModel UpdateUser(string username, string role);
        public Location BindDeskToLocation(int deskId, int locationId);
    }
}
