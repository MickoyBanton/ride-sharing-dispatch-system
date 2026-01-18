using Microsoft.EntityFrameworkCore;
using Moq;
using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Application.Services;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Domain.Enums;
using RideSharingDispatch.Infrastructure.Data;
using RideSharingDispatch.Infrastructure.Repositories;
using Xunit;

namespace RideSharingDispatch.Tests
{
    public class TripRepoTest
    {
        [Fact]
        public async Task GetTripByIdAsync_ShouldReturnTrip_WhenTripExists()
        {
            // Arrange: set up in-memory DB
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TripTestDb")
                .Options;

            using var context = new AppDbContext(options);

            context.Trips.Add(new Trip
            {
                Id = 1,
                PickupLatitude = 12,
                PickupLongitude = 34,
                TripStatus = TripStatus.Requested
            });
            await context.SaveChangesAsync();

            var repo = new TripRepository(context);

            // Act
            var result = await repo.GetTripByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }


        [Fact]
        public async Task GetTripByIdAsync_ShouldReturnNull_WhenTripDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TripMissingDb")
                .Options;

            using var context = new AppDbContext(options);
            var repo = new TripRepository(context);

            var result = await repo.GetTripByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateTripAsync_ShouldStoreTrip()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TripCreateDb")
                .Options;

            using var context = new AppDbContext(options);
            var repo = new TripRepository(context);

            var trip = new Trip
            {
                Id = 2,
                PickupLatitude = 9,
                PickupLongitude = 8,
                TripStatus = TripStatus.Requested
            };

            await repo.CreateTripAsync(trip);

            var saved = await context.Trips.FindAsync(2);

            Assert.NotNull(saved);
            Assert.Equal(TripStatus.Requested, saved.TripStatus);
        }

        [Fact]
        public async Task GetTripsByDriverIdAsync_ReturnsTripsForDriver()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetTripsByDriverIdDb")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Trips.AddRange(
                    new Trip { Id = 1, DriverId = 10, PickupLatitude = 1, PickupLongitude = 2 },
                    new Trip { Id = 2, DriverId = 10, PickupLatitude = 3, PickupLongitude = 4 },
                    new Trip { Id = 3, DriverId = 20, PickupLatitude = 5, PickupLongitude = 6 }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repo = new TripRepository(context);

                var result = await repo.GetTripsByDriverIdAsync(10);

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.All(result, t => Assert.Equal(10, t.DriverId));
            }
        }

        [Fact]
        public async Task GetTripsByRiderIdAsync_ReturnsTripsForRider()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetTripsByRiderIdDb")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Trips.AddRange(
                    new Trip { Id = 1, RiderId = 100, PickupLatitude = 1, PickupLongitude = 2 },
                    new Trip { Id = 2, RiderId = 100, PickupLatitude = 3, PickupLongitude = 4 },
                    new Trip { Id = 3, RiderId = 200, PickupLatitude = 5, PickupLongitude = 6 }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var repo = new TripRepository(context);

                var result = await repo.GetTripsByRiderIdAsync(100);

                Assert.NotNull(result);
                Assert.Equal(2, result.Count);
                Assert.All(result, t => Assert.Equal(100, t.RiderId));
            }
        }



    }
}
