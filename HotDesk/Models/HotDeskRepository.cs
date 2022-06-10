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
            return dbContext.Reservations;
        }
        public Reservation GetReservation(int id)
        {
            return dbContext.Reservations.Find(id);
        }
        public void AddReservation(Reservation reservation)
        {
            dbContext.Reservations.Add(reservation);
            dbContext.SaveChanges();
        }

        public IEnumerable<Desk> GetAllDesks()
        {
            return dbContext.Desks;
        }

        public IEnumerable<Location> GetAllLocations()
        {
            return dbContext.Locations;
        }


        public Desk GetDesk(int id)
        {
            throw new NotImplementedException();
        }

        public Location GetLocation(int id)
        {
            throw new NotImplementedException();
        }

        public void AddDesk(Desk desk)
        {
            throw new NotImplementedException();
        }

        public void AddLocation(Location location)
        {
            throw new NotImplementedException();
        }

        public void RemoveDesk(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveLocation(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveReservation(int id)
        {
            throw new NotImplementedException();
        }

        public Desk UpdateDesk(int id, Desk desk)
        {
            throw new NotImplementedException();
        }

        public Location UpdateLocation(int id, Location location)
        {
            throw new NotImplementedException();
        }

        public Reservation UpdateReservation(int id, Reservation reservation)
        {
            throw new NotImplementedException();
        }
    }
    public interface IHotDeskRepository
    {
        public IEnumerable<Reservation> GetAllReservations();
        public IEnumerable<Desk> GetAllDesks();
        public IEnumerable<Location> GetAllLocations();
        public Reservation GetReservation(int id);
        public Desk GetDesk(int id);
        public Location GetLocation(int id);
        public void AddReservation(Reservation reservation);
        public void AddDesk(Desk desk);
        public void AddLocation(Location location);
        public void RemoveDesk(int id);
        public void RemoveLocation(int id);
        public void RemoveReservation(int id);
        public Desk UpdateDesk(int id, Desk desk);
        public Location UpdateLocation(int id, Location location);
        public Reservation UpdateReservation(int id, Reservation reservation);

    }
}
