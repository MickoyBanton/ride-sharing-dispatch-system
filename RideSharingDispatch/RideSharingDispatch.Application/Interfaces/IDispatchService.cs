using RideSharingDispatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.Interfaces
{
    public interface IDispatchService
    {
        Driver? FindBestDriver(Trip trip);
        IEnumerable<Driver> GetNearbyDrivers(decimal lat, decimal lng, int radiusKm);
    }
}
