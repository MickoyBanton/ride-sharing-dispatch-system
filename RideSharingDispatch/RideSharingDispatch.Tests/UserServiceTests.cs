using Microsoft.AspNetCore.Identity;
using Moq;
using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Application.Services;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Domain.Enums;
using Xunit;

namespace RideSharingDispatch.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IRiderRepository> _mockRiderRepo;
        private readonly Mock<IDriverRepository> _mockDriverRepo;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockRiderRepo = new Mock<IRiderRepository>();
            _mockDriverRepo = new Mock<IDriverRepository>();
            _mockUserRepo = new Mock<IUserRepository>();

            _service = new UserService(
                _mockRiderRepo.Object,
                _mockDriverRepo.Object,
                _mockUserRepo.Object
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

        // ------------------------------
        // USER NOT FOUND
        // ------------------------------
        [Fact]
        public async Task LoginAsync_UserNotFound_ReturnsFailure()
        {
            _mockUserRepo
                .Setup(r => r.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            var result = await _service.LoginAsync("test@email.com", "password");

            Assert.False(result.IsSuccessful);
            Assert.Equal("Invalid email or password", result.ErrorMessage);
        }

        // ------------------------------
        // WRONG PASSWORD
        // ------------------------------
        [Fact]
        public async Task LoginAsync_WrongPassword_ReturnsFailure()
        {
            var user = new User
            {
                Id = 1,
                Email = "test@email.com"
            };

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, "correct-password");

            _mockUserRepo
                .Setup(r => r.GetByEmailAsync(user.Email))
                .ReturnsAsync(user);

            var result = await _service.LoginAsync(user.Email, "wrong-password");

            Assert.False(result.IsSuccessful);
            Assert.Equal("Invalid email or password", result.ErrorMessage);
        }


        // ------------------------------
        // SUCCESSFUL LOGIN
        // ------------------------------
        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsSuccess()
        {
            var user = new User
            {
                Id = 10,
                Email = "test@email.com",
                Role = UserRole.Driver,
            };

            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, "correct-password");

            _mockUserRepo
                .Setup(r => r.GetByEmailAsync(user.Email))
                .ReturnsAsync(user);

            var result = await _service.LoginAsync(user.Email, "correct-password");

            Assert.True(result.IsSuccessful);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.Role, result.Role);
        }



    }



}
