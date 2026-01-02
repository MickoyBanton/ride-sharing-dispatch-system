using Microsoft.EntityFrameworkCore;
using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Domain.Enums;
using RideSharingDispatch.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingDispatch.Infrastructure.Repositories
{
    public class TripRepository: ITripRepository
    {

        private readonly AppDbContext context;
        public TripRepository(AppDbContext context) 
        {
            this.context = context;
        }

        public async Task CreateTripAsync(Trip trip)
        {
            if (trip == null)
                throw new ArgumentNullException(nameof(trip));

            await context.Trips.AddAsync(trip);
            await context.SaveChangesAsync();
        }

        public async Task<Trip?> GetTripByIdAsync(int tripId)
        {
            return await context.Trips.AsNoTracking().FirstOrDefaultAsync(t => t.Id == tripId);
        }

        public async Task<bool> AssignDriverAsync(int driverId, int tripId)
        {
            var trip = await context.Trips.FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null || trip.TripStatus != TripStatus.Requested)
                return false;

            trip.DriverId = driverId;
            trip.TripStatus = TripStatus.Accepted;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTripStatusAsync(int tripId, TripStatus tripStatus)
        {
            var trip = await context.Trips.FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
                return false;

            trip.TripStatus = tripStatus;

            if (tripStatus == TripStatus.Completed)
            {
                trip.CompletedAt = DateTime.UtcNow;
            }

            await context.SaveChangesAsync();
            return true;

        }

        public async Task<IReadOnlyList<Trip>> GetTripsByDriverIdAsync(int driverId)
        {
            return await context.Trips.AsNoTracking().Where(t => t.DriverId == driverId).ToListAsync();
        }

        public async Task<IReadOnlyList<Trip>> GetTripsByRiderIdAsync(int riderId)
        {
            return await context.Trips.AsNoTracking().Where(t => t.RiderId == riderId).ToListAsync();
        }


    }
}
