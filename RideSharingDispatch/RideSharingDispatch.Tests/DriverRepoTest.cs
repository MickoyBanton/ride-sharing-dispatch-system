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
        public async Task AddDriver_AddsUserAndDriverSuccessfully()
        {
            using var context = CreateContext("AddDriverDb");
            var repo = new DriverRepository(context);

            var user = new User { Id = 1, Email = "driver@test.com" };
            var driver = new Driver { Id = 1, IsOnline = false };

            await repo.AddDriver(driver, user);

            Assert.Single(context.Users);
            Assert.Single(context.Drivers);

            var savedDriver = context.Drivers.First();
            Assert.Equal(user.Id, savedDriver.UserId);
        }

        [Fact]
        public async Task AddDriver_Throws_WhenUserIsNull()
        {
            using var context = CreateContext("AddDriverNullUserDb");
            var repo = new DriverRepository(context);

            var driver = new Driver { Id = 1 };

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                repo.AddDriver(driver, null!));
        }


        [Fact]
        public async Task AddDriver_Throws_WhenDriverIsNull()
        {
            using var context = CreateContext("AddDriverNullDriverDb");
            var repo = new DriverRepository(context);

            var user = new User { Id = 1 };

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                repo.AddDriver(null!, user));
        }



        [Fact]
        public async Task RemoveDriver_RemovesDriverAndUser()
        {
            using var context = CreateContext("RemoveDriverDb");

            context.Users.Add(new User { Id = 5 });
            context.Drivers.Add(new Driver { Id = 5, UserId = 5 });
            await context.SaveChangesAsync();

            var repo = new DriverRepository(context);

            await repo.RemoveDriver(5);

            Assert.Empty(context.Users);
            Assert.Empty(context.Drivers);
        }

        [Fact]
        public async Task RemoveDriver_Throws_WhenDriverNotFound()
        {
            using var context = CreateContext("RemoveDriverMissingDb");
            var repo = new DriverRepository(context);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                repo.RemoveDriver(99));
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
