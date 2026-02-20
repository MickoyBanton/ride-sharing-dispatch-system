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
        Task<Trip> CreateTrip(Trip trip);

        Task<AcceptTripResult> AssignDriver(int tripId);

        Task<bool> UpdateTripStatus(int tripId, TripStatus newTripStatus);

        Task<bool> CancelTrip(int tripId);

        Task<bool> CompleteTrip(int tripId);

        Task<Trip?> GetTrip(int tripId);

        Task<IReadOnlyList<Trip>> GetRiderTrips(int riderId);
        Task<IReadOnlyList<Trip>> GetDriverTrips(int driverId);
    }
}
