using Xunit;
using Moq;
using RideSharingDispatch.Application.Services;
using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Entities;

namespace RideSharingDispatch.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IRiderRepository> _mockRiderRepo;
        private readonly Mock<IDriverRepository> _mockDriverRepo;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockRiderRepo = new Mock<IRiderRepository>();
            _mockDriverRepo = new Mock<IDriverRepository>();

            _service = new UserService(
                _mockRiderRepo.Object,
                _mockDriverRepo.Object
            );
        }

        [Fact]
        public async Task RegisterRider_CallsAddRiderAsync()
        {
            var rider = new Rider { Id = 1, UserId = 100 };

            await _service.RegisterRider(rider);

            _mockRiderRepo.Verify(
                r => r.AddRiderAsync(rider),
                Times.Once
            );
        }

        [Fact]
        public async Task UnregisterRider_CallsRemoveRiderAsync()
        {
            var rider = new Rider { Id = 2, UserId = 101 };

            await _service.UnregisterRider(rider);

            _mockRiderRepo.Verify(
                r => r.RemoveRiderAsync(rider),
                Times.Once
            );
        }

        [Fact]
        public async Task RegisterDriver_CallsAddDriver()
        {
            var driver = new Driver { Id = 5, UserId = 200 };

            await _service.RegisterDriver(driver);

            _mockDriverRepo.Verify(
                d => d.AddDriver(driver),
                Times.Once
            );
        }


        [Fact]
        public async Task UnregisterDriver_CallsRemoveDriver()
        {
            var driver = new Driver { Id = 6, UserId = 201 };

            await _service.UnregisterDriver(driver);

            _mockDriverRepo.Verify(
                d => d.RemoveDriver(driver),
                Times.Once
            );
        }
    }



}
