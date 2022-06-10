namespace HotDesk.Models
{
    public class Desk
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Location Location { get; set; }
    }
}
