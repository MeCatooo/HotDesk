namespace HotDesk.Models
{
    public class MemoryRepository
    {
        private static Dictionary<int,Reservation> _reservations = new Dictionary<int, Reservation>();
        private static int _nextId = 1;
        public void Add(Reservation reservation)
        {
            reservation.Id = _nextId;
            _reservations.Add(_nextId, reservation);
            _nextId++;
        }
        public Reservation Get(int id)
        {
            if (_reservations.ContainsKey(id))
            {
                return _reservations[id];
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<Reservation> GetAll()
        {
            return _reservations.Values;
        }
    }
}
