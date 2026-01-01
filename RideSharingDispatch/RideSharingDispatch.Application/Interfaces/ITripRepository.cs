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
        Task CreateTripAsync(Trip trip);
        Task<bool> AssignDriverAsync(int driverId, int tripId);
        Task<bool> UpdateTripStatusAsync(TripStatus tripStatus, int tripId);
        Task<IReadOnlyList<Trip>> GetTripsByDriverIdAsync(int driverId);
        Task<IReadOnlyList<Trip>> GetTripsByRiderIdAsync(int riderId);
    }
}
