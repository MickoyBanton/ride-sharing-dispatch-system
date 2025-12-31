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
        Task CreateTrip(Trip trip);
        Task AssignDriver(Driver driver);
        Task UpdateTripStatus(TripStatus TripStatus);
        Task<IEnumerable<Trip>> GetActiveTrip();
        Task<IEnumerable<Trip>> GetTripsByUserId(int UserId);
    }
}
