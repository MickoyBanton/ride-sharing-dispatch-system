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
    public class RiderRepoTest
    {
        private AppDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new AppDbContext(options);
        }


        [Fact]
        public async Task AddRiderAsync_AddsUserAndRiderSuccessfully()
        {
            using var context = CreateContext("AddRiderDb");
            var repo = new RiderRepository(context);

            var user = new User { Id = 10, Email = "rider@test.com" };
            var rider = new Rider { Id = 10 };

            await repo.AddRiderAsync(rider, user);

            Assert.Single(context.Users);
            Assert.Single(context.Riders);
            Assert.Equal(user.Id, context.Riders.First().UserId);
        }

        [Fact]
        public async Task AddRiderAsync_Throws_WhenRiderIsNull()
        {
            using var context = CreateContext("AddRiderNullDb");
            var repo = new RiderRepository(context);

            var user = new User { Id = 1 };

            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                repo.AddRiderAsync(null!, user));
        }

        [Fact]
        public async Task RemoveRiderAsync_RemovesRiderAndUser()
        {
            using var context = CreateContext("RemoveRiderDb");

            context.Users.Add(new User { Id = 20 });
            context.Riders.Add(new Rider { Id = 20, UserId = 20 });
            await context.SaveChangesAsync();

            var repo = new RiderRepository(context);

            await repo.RemoveRiderAsync(20);

            Assert.Empty(context.Users);
            Assert.Empty(context.Riders);
        }

        [Fact]
        public async Task RemoveRiderAsync_Throws_WhenRiderNotFound()
        {
            using var context = CreateContext("RemoveRiderMissingDb");
            var repo = new RiderRepository(context);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                repo.RemoveRiderAsync(77));
        }



        [Fact]
        public async Task GetRiderAsync_ReturnsCorrectRider()
        {
            using var context = CreateContext("GetRiderDb");
            context.Riders.Add(new Rider { Id = 1, UserId = 55, Name = "Jamie" });
            await context.SaveChangesAsync();

            var repo = new RiderRepository(context);

            var rider = await repo.GetRiderAsync(55);

            Assert.NotNull(rider);
            Assert.Equal("Jamie", rider.Name);
        }

        [Fact]
        public async Task GetAllRidersAsync_ReturnsAllRiders()
        {
            using var context = CreateContext("GetAllRidersDb");
            context.Riders.AddRange(
                new Rider { Id = 1, UserId = 11 },
                new Rider { Id = 2, UserId = 12 },
                new Rider { Id = 3, UserId = 13 }
            );
            await context.SaveChangesAsync();

            var repo = new RiderRepository(context);

            var riders = await repo.GetAllRidersAsync();

            Assert.Equal(3, riders.Count());
        }



    }
}
