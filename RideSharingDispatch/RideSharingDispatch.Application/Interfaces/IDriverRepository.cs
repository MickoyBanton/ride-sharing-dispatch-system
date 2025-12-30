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
        void AddDriver(Driver driver);
        void RemoveDriver(int UserId);
        void UpdateDriverLocation (decimal Latitude, decimal Longitude);
        void ChangeDriverAvailability (bool IsOnline);
        Driver GetDriver(int UserId);

        IEnumerable <Driver> GetAllDrivers ();

        IEnumerable<Driver> GetOnlineDrivers ();
        IEnumerable<Driver> GetNearbyDrivers ();


    }
}
