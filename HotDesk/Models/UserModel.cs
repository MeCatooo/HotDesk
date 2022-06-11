using HotDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JwtApp.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}