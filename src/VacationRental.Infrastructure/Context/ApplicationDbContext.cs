using Microsoft.EntityFrameworkCore;
using VacationRental.Core.Models.Domain;

namespace VacationRental.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Rental> Rental { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasIndex(i => i.Start);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Rental)
                .WithMany(b => b.Bookings)
                .HasForeignKey(p => p.RentalId);
        }
    }
}