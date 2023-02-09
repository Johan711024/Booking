using Booking.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Booking.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GymClass> GymClass { get; set; } = default!;
        public DbSet<ApplicationUserGymClass> ApplicationUserGymClass { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FluentAPI goes here
            base.OnModelCreating(modelBuilder);

            // Defined composite key for junction entity (kopplingstabell)
            modelBuilder.Entity<ApplicationUserGymClass>()
                .HasKey(e => new { e.ApplicationUserId, e.GymClassId });
        }
    }
}