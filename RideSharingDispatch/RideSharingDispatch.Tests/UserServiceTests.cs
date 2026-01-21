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

        // ---------------- RIDER ----------------

        [Fact]
        public async Task RegisterRider_CallsAddRiderAsync_WithCorrectArguments()
        {
            var user = new User { Id = 100 };
            var rider = new Rider { Id = 1 };

            await _service.RegisterRider(rider, user);

            _mockRiderRepo.Verify(
                r => r.AddRiderAsync(rider, user),
                Times.Once
            );
        }

        [Fact]
        public async Task UnregisterRider_CallsRemoveRiderAsync_WithUserId()
        {
            int userId = 101;

            await _service.UnregisterRider(userId);

            _mockRiderRepo.Verify(
                r => r.RemoveRiderAsync(userId),
                Times.Once
            );
        }

        // ---------------- DRIVER ----------------

        [Fact]
        public async Task RegisterDriver_CallsAddDriver_WithCorrectArguments()
        {
            var user = new User { Id = 200 };
            var driver = new Driver { Id = 5 };

            await _service.RegisterDriver(driver, user);

            _mockDriverRepo.Verify(
                d => d.AddDriver(driver, user),
                Times.Once
            );
        }

        [Fact]
        public async Task UnregisterDriver_CallsRemoveDriver_WithUserId()
        {
            int userId = 201;

            await _service.UnregisterDriver(userId);

            _mockDriverRepo.Verify(
                d => d.RemoveDriver(userId),
                Times.Once
            );
        }



    }



}
