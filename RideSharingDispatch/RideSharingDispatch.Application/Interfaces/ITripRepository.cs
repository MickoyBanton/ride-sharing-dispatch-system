using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.Interfaces
{
    public interface ITripRepository
    {
        void CreateTrip(Trip trip);

        void AssignDriver(Driver driver);
        void UpdateTripStatus(TripStatus TripStatus);

        IEnumerable<Trip> GetActiveTrip();
        IEnumerable<Trip> GetTripsByUserId(int UserId);
    }
}
