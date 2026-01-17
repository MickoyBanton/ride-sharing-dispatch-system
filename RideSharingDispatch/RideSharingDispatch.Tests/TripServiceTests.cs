using Moq;
using Xunit;
using RideSharingDispatch.Application.Services;
using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Domain.Enums;

namespace RideSharingDispatch.Tests
{
    public class TripServiceTests
    {
        [Fact]
        public async Task AssignDriver_ShouldReturnFalse_WhenTripNotFound()
        {
            // Arrange
            var mockTripRepo = new Mock<ITripRepository>();
            var mockDriverRepo = new Mock<IDriverRepository>();

            mockTripRepo.Setup(r => r.GetTripByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Trip)null); // trip does not exist

            var service = new TripService(mockTripRepo.Object, mockDriverRepo.Object);

            // Act
            var result = await service.AssignDriver(123);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AssignDriver_ShouldAssignClosestOnlineDriver()
        {
            // Arrange
            var trip = new Trip
            {
                Id = 1,
                PickupLatitude = 10,
                PickupLongitude = 20,
                VehicleType = VehicleType.Premium,
                TripStatus = TripStatus.Requested
            };

            var drivers = new List<Driver>()
            {
                new Driver { Id = 5, CurrentLatitude = 11, CurrentLongitude = 21, IsOnline = true, VehicleType = VehicleType.Premium },
                new Driver { Id = 6, CurrentLatitude = 50, CurrentLongitude = 60, IsOnline = true, VehicleType = VehicleType.Premium }
            };

            var mockTripRepo = new Mock<ITripRepository>();
            var mockDriverRepo = new Mock<IDriverRepository>();

            mockTripRepo.Setup(r => r.GetTripByIdAsync(1)).ReturnsAsync(trip);
            mockDriverRepo.Setup(r => r.GetAllDrivers()).ReturnsAsync(drivers);
            mockTripRepo.Setup(r => r.AssignDriverAsync(5, 1)).ReturnsAsync(true);
            mockTripRepo.Setup(r => r.UpdateTripStatusAsync(1, TripStatus.Accepted)).ReturnsAsync(true);

            var service = new TripService(mockTripRepo.Object, mockDriverRepo.Object);

            // Act
            var result = await service.AssignDriver(1);

            // Assert
            Assert.True(result);

            // Ensure closest driver was used
            mockTripRepo.Verify(r => r.AssignDriverAsync(5, 1), Times.Once);
        }


        [Fact]
        public async Task AssignDriver_ShouldReturnFalse_WhenNoDriversAvailable()
        {
            var trip = new Trip
            {
                Id = 1,
                PickupLatitude = 10,
                PickupLongitude = 20,
                VehicleType = VehicleType.Standard,
                TripStatus = TripStatus.Requested
            };

            var mockTripRepo = new Mock<ITripRepository>();
            var mockDriverRepo = new Mock<IDriverRepository>();

            mockTripRepo.Setup(r => r.GetTripByIdAsync(1)).ReturnsAsync(trip);
            mockDriverRepo.Setup(r => r.GetAllDrivers()).ReturnsAsync(new List<Driver>()); // no drivers

            var service = new TripService(mockTripRepo.Object, mockDriverRepo.Object);

            var result = await service.AssignDriver(1);

            Assert.False(result);
        }


    }
}
