namespace HotDesk.Models
{
    public static class ReservationLogic
    {
        //is there a reservation for desk in given time span
        public static bool IsDeskReserved(Desk desk, DateTime startTime, DateTime endTime, IHotDeskRepository context)
        {
            HashSet<Desk> occupied = new HashSet<Desk>();
            foreach (Reservation reservation in context.GetAllReservations())
            {
                //if reservation exists in given time span
                if ((startTime <= reservation.From && endTime >= reservation.From) || (startTime <= reservation.To && endTime >= reservation.To) || (reservation.From <= startTime && reservation.To >= endTime) || (reservation.From <= startTime && reservation.To >= endTime))
                {
                    occupied.Add(reservation.desk);
                }
            }
            if (occupied.Any(a => a.Id == desk.Id))
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public static List<Desk> FreeDesks(int locationId,DateTime startTime, DateTime endTime, IHotDeskRepository context)
        {
            HashSet<Desk> occupied = new HashSet<Desk>();
            foreach (Reservation reservation in context.GetAllReservations())
            {
                //if reservation exists in given time span
                if ((startTime <= reservation.From && endTime >= reservation.From) || (startTime <= reservation.To && endTime >= reservation.To) || (reservation.From <= startTime && reservation.To >= endTime) || (reservation.From <= startTime && reservation.To >= endTime))
                {
                    occupied.Add(reservation.desk);
                }
            }
            Location location = context.GetLocation(locationId);
            if (ReferenceEquals(location, null))
            {
                return null;
            }
            return location.Desks.Except(occupied).ToList();
        }
        public static List<Desk> OccupiedDesks(int locationId, DateTime startTime, DateTime endTime, IHotDeskRepository context)
        {
            HashSet<Desk> occupied = new HashSet<Desk>();
            foreach (Reservation reservation in context.GetAllReservations())
            {
                //if reservation exists in given time span
                if ((startTime <= reservation.From && endTime >= reservation.From) || (startTime <= reservation.To && endTime >= reservation.To) || (reservation.From <= startTime && reservation.To >= endTime) || (reservation.From <= startTime && reservation.To >= endTime))
                {
                    occupied.Add(reservation.desk);
                }
            }
            Location location = context.GetLocation(locationId);
            if(ReferenceEquals(location,null))
            {
                return null;
            }
            return location.Desks.Where(a => occupied.Any(b => b.Id == a.Id)).ToList();
        }
    }
}
