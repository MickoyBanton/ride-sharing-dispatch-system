using Microsoft.EntityFrameworkCore;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Infrastructure.Data;
using RideSharingDispatch.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Tests
{
    public class DriverRepoTest
    {
        private AppDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddDriver_AddsDriverSuccessfully()
        {
            using var context = CreateContext("AddDriverDb");
            var repo = new DriverRepository(context);

            var driver = new Driver { Id = 1, Name = "John", IsOnline = false };

            await repo.AddDriver(driver);
            await context.SaveChangesAsync();

            Assert.Equal(1, context.Drivers.Count());
        }

        [Fact]
        public async Task RemoveDriver_RemovesDriverSuccessfully()
        {
            using var context = CreateContext("RemoveDriverDb");
            var driver = new Driver { Id = 1, Name = "John", IsOnline = false };
            context.Drivers.Add(driver);
            await context.SaveChangesAsync();

            var repo = new DriverRepository(context);

            await repo.RemoveDriver(driver);
            await context.SaveChangesAsync();

            Assert.Empty(context.Drivers);
        }

        [Fact]
        public async Task UpdateDriverLocation_UpdatesLocationSuccessfully()
        {
            using var context = CreateContext("UpdateLocationDb");
            context.Drivers.Add(new Driver { Id= 5, UserId = 5, CurrentLatitude = 0, CurrentLongitude = 0 });
            await context.SaveChangesAsync();

            var repo = new DriverRepository(context);

            bool updated = await repo.UpdateDriverLocation(18.0m, -77.0m, 5);

            Assert.True(updated);
            var driver = context.Drivers.First(d => d.Id == 5);
            Assert.Equal(18.0m, driver.CurrentLatitude);
            Assert.Equal(-77.0m, driver.CurrentLongitude);
        }

        [Fact]
        public async Task ChangeDriverAvailability_UpdatesStatusSuccessfully()
        {
            using var context = CreateContext("ChangeAvailabilityDb");
            context.Drivers.Add(new Driver { Id = 10, UserId = 10, Name = "John", IsOnline = false });
            await context.SaveChangesAsync();

            var repo = new DriverRepository(context);

            bool changed = await repo.ChangeDriverAvailability(true, 10);

            Assert.True(changed);
            Assert.True(context.Drivers.First(d => d.Id == 10).IsOnline);
        }

        [Fact]
        public async Task GetDriver_ReturnsCorrectDriver()
        {
            using var context = CreateContext("GetDriverDb");
            context.Drivers.Add(new Driver { Id = 20, UserId = 20, Name = "Mike" });
            await context.SaveChangesAsync();

            var repo = new DriverRepository(context);

            var driver = await repo.GetDriver(20);

            Assert.NotNull(driver);
            Assert.Equal("Mike", driver.Name);
        }

        [Fact]
        public async Task GetOnlineDrivers_ReturnsOnlyOnlineDrivers()
        {
            using var context = CreateContext("GetOnlineDriversDb");
            context.Drivers.AddRange(
                new Driver { Id = 1, IsOnline = true },
                new Driver { Id = 2, IsOnline = true },
                new Driver { Id = 3, IsOnline = false }
            );
            await context.SaveChangesAsync();

            var repo = new DriverRepository(context);

            var result = await repo.GetOnlineDrivers();

            Assert.Equal(2, result.Count());
            Assert.All(result, d => Assert.True(d.IsOnline));
        }

        


    }
}
