using JwtApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotDesk.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        [JsonIgnore]
        public virtual Location location { get; set; }
        public virtual Desk desk { get; set; }
        public virtual UserModel user { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
    }
}
