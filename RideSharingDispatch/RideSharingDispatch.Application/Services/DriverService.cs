using RideSharingDispatch.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.Services
{
    public class DriverService : IDriverService
    {

        private readonly IDriverRepository _driverRepository;
        public DriverService(IDriverRepository driverRepository) 
        {
            _driverRepository = driverRepository;
        }

        public async Task<bool> SetOnlineStatus(bool isOnline, int userId)
        {
            bool result = await _driverRepository.ChangeDriverAvailability(isOnline, userId);
            return result;
        }

        public async Task<bool> UpdateLocation(decimal latitude, decimal longitude, int userId)
        {
            bool result = await _driverRepository.UpdateDriverLocation(latitude, longitude, userId);
            return result;
        }
    }
}
