using System.ComponentModel.DataAnnotations;

namespace HotDesk.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public Location location { get; set; }
        public Desk desk { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        //to do: Dodać imię i nazwisko do rezerwacji, aby po tych danych weryfikować
    }
}
