using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Application.Interfaces
{
    public interface ITripService
    {
        Task CreateTrip(Trip trip);

        Task<bool> AssignDriver(int tripId);

        void AcceptTrip(int tripId, int driverId);

        Task<bool> UpdateTripStatus(int tripId, TripStatus newTripStatus);

        void CancelTrip(int tripId);

        void CompleteTrip(int tripId);

        Task<Trip?> GetTrip(int tripId);
    }
}
