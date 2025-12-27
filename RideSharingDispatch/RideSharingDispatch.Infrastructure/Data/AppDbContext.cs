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
    }
}
