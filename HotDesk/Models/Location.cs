namespace HotDesk.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Desk> Desks { get; set; }
    }
}
