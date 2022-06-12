using HotDesk.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotDeskTests
{
    internal class ReservationControllerTest
    {
        private HotDeskRepository GetContext()
        {
            var options = new DbContextOptionsBuilder<HotDeskDbContext>().UseInMemoryDatabase("HotDesk")
                .Options;
            var databaseContext = new HotDeskDbContext(options);
            databaseContext.Database.EnsureCreated();
            var repository = new HotDeskRepository(databaseContext);
            return repository;
        }
        
    }
}
