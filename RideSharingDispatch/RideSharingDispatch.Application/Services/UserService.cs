using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RideSharingDispatch.Application.Services
{
    public class UserService: IUserService
    {
        private readonly IRiderRepository riderRepository;
        private readonly IDriverRepository driverRepository;
        private readonly IUserRepository userRepository;

        public UserService(IRiderRepository riderRepository, IDriverRepository driverRepository, IUserRepository userRepository) 
        {
            this.riderRepository = riderRepository;
            this.driverRepository = driverRepository;
            this.userRepository = userRepository;
        }

        public Task RegisterRider(Rider rider, User user)
        {
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, user.PasswordHash);

            return riderRepository.AddRiderAsync(rider, user);
        }

        public Task UnregisterRider(int userId)
        {
            return riderRepository.RemoveRiderAsync(userId);
        }

        public Task RegisterDriver(Driver driver, User user)
        {
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, user.PasswordHash);

            return driverRepository.AddDriver(driver, user);
        }

        public Task UnregisterDriver(int userId)
        {
            return driverRepository.RemoveDriver(userId);
        }

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            var user = await userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                return new LoginResult
                {
                    IsSuccessful = false,
                    ErrorMessage = "Invalid email or password"
                };
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return new LoginResult
                {
                    IsSuccessful = false,
                    ErrorMessage = "Invalid email or password"
                };
            }

            return new LoginResult
            {
                IsSuccessful = true,
                UserId = user.Id,
                Role = user.Role
            };
        }

    }
}
