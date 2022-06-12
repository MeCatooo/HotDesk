using JwtApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotDesk.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        [JsonIgnore]
        public virtual Location Location { get; set; }
        public virtual Desk Desk { get; set; }
        public virtual UserModel User { get; set; }
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
    }
}
