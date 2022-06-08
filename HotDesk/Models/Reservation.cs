namespace HotDesk.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        //public Location location { get; set; }
        //public Desk desk { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

    }
}
