using HotDesk.Models;
using System.Text.Json.Serialization;

namespace JwtApp.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}