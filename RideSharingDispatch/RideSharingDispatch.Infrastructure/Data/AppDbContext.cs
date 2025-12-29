using Microsoft.EntityFrameworkCore;
using RideSharingDispatch.Domain.Entities;

namespace RideSharingDispatch.Infrastructure.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Driver> Drivers => Set<Driver>();
        public DbSet<Rider> Riders => Set<Rider>();
        public DbSet<Trip> Trips => Set<Trip>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.Property(d => d.CurrentLatitude)
                      .HasPrecision(9, 6);

                entity.Property(d => d.CurrentLongitude)
                      .HasPrecision(9, 6);
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.Property(t => t.PickupLatitude)
                      .HasPrecision(9, 6);

                entity.Property(t => t.PickupLongitude)
                      .HasPrecision(9, 6);

                entity.Property(t => t.DestinationLatitude)
                      .HasPrecision(9, 6);

                entity.Property(t => t.DestinationLongitude)
                      .HasPrecision(9, 6);

                entity.Property(t => t.Fare)
                      .HasPrecision(10, 2);
            });
        }

    }
}
