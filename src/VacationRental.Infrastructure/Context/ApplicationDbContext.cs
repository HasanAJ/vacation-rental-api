using Microsoft.EntityFrameworkCore;
using VacationRental.Core.Models.Domain;

namespace VacationRental.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Rental> Rentals { get; set; }
        public virtual DbSet<Unit> Units { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Unit>()
                .HasOne(b => b.Rental)
                .WithMany(b => b.AllUnits)
                .HasForeignKey(p => p.RentalId);

            modelBuilder.Entity<Booking>()
                .HasIndex(i => i.Start);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Unit)
                .WithMany(b => b.Bookings)
                .HasForeignKey(p => p.UnitId);
        }
    }
}