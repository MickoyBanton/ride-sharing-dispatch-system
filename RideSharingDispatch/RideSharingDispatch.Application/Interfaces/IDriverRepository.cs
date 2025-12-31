using RideSharingDispatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.Interfaces
{
    public interface IDriverRepository
    {
        Task AddDriver(Driver driver);
        Task RemoveDriver(Driver driver);
        Task <bool> UpdateDriverLocation (decimal latitude, decimal longitude, int userId);
        Task<bool> ChangeDriverAvailability (bool isOnline, int userId);
        Task <Driver?> GetDriver(int userId);

        Task<IEnumerable<Driver>> GetAllDrivers ();

        Task<IEnumerable<Driver>> GetOnlineDrivers ();
        Task<IEnumerable<Driver>> GetNearbyDrivers (decimal latitude, decimal longitude);


    }
}
