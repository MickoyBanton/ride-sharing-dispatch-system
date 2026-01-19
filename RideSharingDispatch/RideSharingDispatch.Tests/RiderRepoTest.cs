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
        public async Task AddRiderAsync_AddsRiderSuccessfully()
        {
            using var context = CreateContext("AddRiderDb");
            var repo = new RiderRepository(context);

            var rider = new Rider { Id = 1, UserId = 1, Name = "Alex" };

            await repo.AddRiderAsync(rider);
            await context.SaveChangesAsync();

            Assert.Equal(1, context.Riders.Count());
        }

        [Fact]
        public async Task RemoveRiderAsync_RemovesRiderSuccessfully()
        {
            using var context = CreateContext("RemoveRiderDb");
            var rider = new Rider { Id = 1, UserId = 101 };
            context.Riders.Add(rider);
            await context.SaveChangesAsync();

            var repo = new RiderRepository(context);

            await repo.RemoveRiderAsync(rider);
            await context.SaveChangesAsync();

            Assert.Empty(context.Riders);
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
