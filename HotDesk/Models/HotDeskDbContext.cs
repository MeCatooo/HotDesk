using JwtApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotDesk.Models
{
    public class HotDeskDbContext : DbContext
    {
        public HotDeskDbContext(DbContextOptions<HotDeskDbContext> options) : base(options)
        {
            
        }

        public DbSet<Desk> Desks { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<UserModel> users { get; set; }
    }
}
