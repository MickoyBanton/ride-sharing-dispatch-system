using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.Services
{
    public class UserService: IUserService
    {
        private readonly IRiderRepository riderRepository;
        private readonly IDriverRepository driverRepository;

        public UserService(IRiderRepository riderRepository, IDriverRepository driverRepository) 
        {
            this.riderRepository = riderRepository;
            this.driverRepository = driverRepository;
        }

        public Task RegisterRider(Rider rider, User user)
        {
            return riderRepository.AddRiderAsync(rider, user);
        }

        public Task UnregisterRider(int userId)
        {
            return riderRepository.RemoveRiderAsync(userId);
        }

        public Task RegisterDriver(Driver driver, User user)
        {
            return driverRepository.AddDriver(driver, user);
        }

        public Task UnregisterDriver(int userId)
        {
            return driverRepository.RemoveDriver(userId);
        }
    }
}
