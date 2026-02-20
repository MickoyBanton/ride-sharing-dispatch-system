using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Domain.Enums;

namespace RideSharingDispatch.Application.Services
{
    public class TripService: ITripService
    {

        private readonly ITripRepository tripRepository;
        private readonly IDriverRepository driverRepository;
        public TripService (ITripRepository tripRepository, IDriverRepository driverRepository)
        {
            this.tripRepository = tripRepository;
            this.driverRepository = driverRepository;
        }

        public Task <Trip> CreateTrip(Trip request)
        {
            // add fare calulation

            return tripRepository.CreateTripAsync(request);
        }

        public async Task<AcceptTripResult> AssignDriver(int tripId)
        {
            Trip? trip = await GetTrip(tripId);

            if (trip == null)
                return AcceptTripResult.TripNotFound;

            decimal distance;
            decimal closestDistance = decimal.MaxValue;
            Driver? closestDriver = null;
            decimal pickupLatitude = trip.PickupLatitude;
            decimal pickupLongitude = trip.PickupLongitude;

            IEnumerable<Driver> drivers = await driverRepository.GetAllDrivers();

            drivers = drivers.Where(d => d.IsOnline == true && d.VehicleType == trip.VehicleType).ToList();
            

            foreach (Driver driver in drivers)
            {
                distance = CalculateDistance(pickupLongitude, pickupLongitude, driver.CurrentLatitude, driver.CurrentLongitude);
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDriver = driver;
                }
            }

            if (closestDriver == null)
            {
                return AcceptTripResult.NoAvailableDriver;
            }

            AcceptTripResult assigned = await tripRepository.AssignDriverAsync(closestDriver.UserId, tripId);

            return assigned;

        }

        public async Task<bool> CancelTrip(int tripId)
        {
            var trip = await tripRepository.GetTripByIdAsync(tripId);


            if (trip == null)
            {
                return false;
            }

            return await tripRepository.UpdateTripStatusAsync(tripId, TripStatus.Cancelled);
        }

        public async Task<bool> CompleteTrip(int tripId)
        {
            var trip = await tripRepository.GetTripByIdAsync(tripId);
            

            if (trip == null)
            {
                return false;
            }

            return await tripRepository.UpdateTripStatusAsync(tripId, TripStatus.Completed);

        }

        public Task<Trip?> GetTrip(int tripId)
        {
            return tripRepository.GetTripByIdAsync(tripId);
        }

        public async Task<bool> UpdateTripStatus(int tripId, TripStatus newTripStatus)
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

        public Task<IReadOnlyList<Trip>> GetRiderTrips(int riderId)
        {
            return tripRepository.GetTripsByRiderIdAsync(riderId);
        }

        public Task<IReadOnlyList<Trip>> GetDriverTrips(int driverId)
        {
            return tripRepository.GetTripsByDriverIdAsync(driverId);
        }

        private decimal CalculateDistance(decimal pickupLongitude, decimal pickupLatitude, decimal driverLongitude, decimal driverLatitude)
        {
            return Math.Abs(pickupLatitude - driverLatitude) + Math.Abs(pickupLongitude - driverLongitude);
        }

        
    }
}
