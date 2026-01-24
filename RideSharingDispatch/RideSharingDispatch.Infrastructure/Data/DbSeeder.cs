using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Domain.Enums;

namespace RideSharingDispatch.Infrastructure.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Users.AnyAsync())
                return; // DB already seeded

            var passwordHasher = new PasswordHasher<User>();

            // USERS
            var riderUser = new User
            {
                Email = "rider@test.com",
                Role = UserRole.Rider
            };
            riderUser.PasswordHash = passwordHasher.HashPassword(riderUser, "Password123!");

            var driverUser = new User
            {
                Email = "driver@test.com",
                Role = UserRole.Driver
            };
            driverUser.PasswordHash = passwordHasher.HashPassword(driverUser, "Password123!");

            context.Users.AddRange(riderUser, driverUser);
            await context.SaveChangesAsync();

            // RIDER
            context.Riders.Add(new Rider
            {
                UserId = riderUser.Id,
                Name = "Test Rider"
            });

            // DRIVER
            context.Drivers.Add(new Driver
            {
                UserId = driverUser.Id,
                Name = "Test Driver",
                IsOnline = true,
                VehicleType = VehicleType.Standard,
                CurrentLatitude = 18.0179m,
                CurrentLongitude = -76.8099m
            });

            await context.SaveChangesAsync();
        }
    }
}

