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

        public Task RegisterRider(Rider rider)
        {
            return riderRepository.AddRiderAsync(rider);
        }

        public Task UnregisterRider(Rider rider)
        {
            return riderRepository.RemoveRiderAsync(rider);
        }

        public Task RegisterDriver(Driver driver)
        {
            return driverRepository.AddDriver(driver);
        }

        public Task UnregisterDriver(Driver driver)
        {
            return driverRepository.RemoveDriver(driver);
        }
    }
}
