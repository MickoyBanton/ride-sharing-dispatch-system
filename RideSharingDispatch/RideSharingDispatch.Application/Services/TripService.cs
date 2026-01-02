using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Enums;

namespace RideSharingDispatch.Application.Services
{
    public class TripService
    {

        private readonly ITripRepository tripRepository;

        public TripService (ITripRepository tripRepository)
        {
            this.tripRepository = tripRepository;
        }

        public async Task<bool>ChangeTripStatus(int tripId, TripStatus newTripStatus)
        {
            var trip = await tripRepository.GetTripByIdAsync (tripId);

            if (trip == null)
                return false;


            bool IsValid = IsTripStatusValid(trip.TripStatus, newTripStatus);

            if (!IsValid ) return false;

            return await tripRepository.UpdateTripStatusAsync(tripId, newTripStatus);

        }

        private bool IsTripStatusValid(TripStatus currentTripStatus, TripStatus newTripStatus) 
        {
            if (currentTripStatus == TripStatus.Requested)
            {
                if (newTripStatus != TripStatus.Accepted && newTripStatus != TripStatus.Cancelled)
                    return false;

                return true;
            }

            if (currentTripStatus == TripStatus.Accepted)
            {
                if (newTripStatus != TripStatus.Arrived && newTripStatus != TripStatus.Cancelled)
                    return false;

                return true;
            }

            if (currentTripStatus == TripStatus.Arrived)
            {
                if (newTripStatus != TripStatus.InProgress && newTripStatus != TripStatus.Cancelled)
                    return false;

                return true;
            }

            if (currentTripStatus == TripStatus.InProgress)
            {
                if (newTripStatus != TripStatus.Completed)
                    return false;

                return true;
            }

            if (currentTripStatus == TripStatus.Completed)
            {
                return false;
            }

            return false;


        }
    }
}
