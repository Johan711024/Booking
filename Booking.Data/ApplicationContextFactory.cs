using Booking.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Booking.Data
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>();



            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-Booking-99fbb771-26f8-459e-a2e1-f1b77e24b9f0;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new ApplicationDbContext(options.Options);

        }
    }
}