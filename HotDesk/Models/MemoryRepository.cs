namespace HotDesk.Models
{
    public class MemoryRepository
    {
        private static List<Reservation> _reservations = new List<Reservation>();
        private static List<Location> _locations = new List<Location>();
        private static List<Desk> _desks = new List<Desk>();
        
        public void AddReservation(Reservation reservation)
        {
            if (ReferenceEquals(null,reservation))
            {
                throw new ArgumentNullException(nameof(reservation));
            }
            reservation.Id = _reservations.Count();
            _reservations.Add(reservation);
        }
        public void AddLocation(Location location)
        {
            if (ReferenceEquals(null, location))
            {
                throw new ArgumentNullException(nameof(location));
            }
            location.Id = _locations.Count();
            _locations.Add(location);
        }
        //add desk
        public void AddDesk(Desk desk, int locationID)
        {
            if (ReferenceEquals(null, desk))
            {
                throw new ArgumentNullException(nameof(desk));
            }
            desk.Id = _locations.Count();
            _desks.Add(desk);
        }
        public Reservation GetReservation(int id)
        {
            if (_reservations.ElementAtOrDefault(id) !=null)
            {
                return _reservations[id];
            }
            else
            {
                return null;
            }
        }
        public Location GetLocation(int id)
        {
            if (_reservations.ElementAtOrDefault(id) != null)
            {
                return _locations[id];
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            return _reservations;
        }
        public IEnumerable<Location> GetAllLocations()
        {
            return _locations;
        }
        public Reservation UpdateReservation(int id, Reservation reservation)
        {
            if (ReferenceEquals(null, reservation))
            {
                throw new ArgumentNullException(nameof(reservation));
            }
            if (id < 0 || id >= _reservations.Count())
            {
                throw new ArgumentException();
            }
            _reservations[id] = reservation;
            return reservation;
        }
        
    }
}
