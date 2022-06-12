using System.Text.Json.Serialization;

namespace HotDesk.Models
{
    public class Desk
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Unavailable { get; set; }
        [JsonIgnore]
        public virtual Location Location { get; set; }
    }
}
