using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.Services
{
    public class DispatchService: IDispatchService
    {
        public Driver? FindBestDriver(Trip trip)
        {
            
        }

        public IEnumerable<Driver> GetNearbyDrivers(decimal lat, decimal lng, int radiusKm)
        {

        }
    }
}
