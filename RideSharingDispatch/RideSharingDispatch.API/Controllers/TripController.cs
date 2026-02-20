using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSharingDispatch.Application.DTOs;
using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Domain.Enums;
using System.Security.Claims;

namespace RideSharingDispatch.API.Controllers
{
    [Authorize]
    [Route("api/trips")]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly IDriverService _driverService;

        public TripController(ITripService tripService, IDriverService driverService)
        {
            _tripService = tripService;
            _driverService = driverService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripRequest request)
        {

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Console.WriteLine("RiderId:", userId);

            var trip = new Trip
            {
                RiderId = userId,
                PickupLatitude = request.PickupLatitude,
                PickupLongitude = request.PickupLongitude,
                DestinationLatitude = request.DestinationLatitude,
                DestinationLongitude = request.DestinationLongitude,
                VehicleType = request.VehicleType,
                TripStatus = TripStatus.Requested
            };

            Trip createdTrip = await _tripService.CreateTrip(trip);

            return Ok(createdTrip);
        }

        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetTrip(int tripId)
        {
            var trip = await _tripService.GetTrip(tripId);

            if (trip == null)
                return NotFound();

            return Ok(trip);
        }

        [HttpGet("rider")]
        public async Task<IActionResult> GetRiderTrips()
        {
            int riderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var trips = await _tripService.GetRiderTrips(riderId);

            if (trips == null)
                return NotFound();

            return Ok(trips);
        }

        [HttpGet("driver")]
        public async Task<IActionResult> GetDriverTrips()
        {
            int driverId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var trips = await _tripService.GetDriverTrips(driverId);

            if (trips == null)
                return NotFound();

            return Ok(trips);
        }



        [HttpPost("{tripId}/cancel")]
        public async Task<IActionResult> CancelTrip(int tripId)
        {
            var result = await _tripService.CancelTrip(tripId);
            return Ok("Trip cancelled");
        }

        [HttpPost("{tripId}/assign")]
        public async Task<IActionResult> AssignTrip(int tripId)
        {
            var result = await _tripService.AssignDriver(tripId);

            return result switch
            {
                AcceptTripResult.Success =>
                    Ok("Trip accepted"),

                AcceptTripResult.TripNotFound =>
                    NotFound("Trip not found"),

                AcceptTripResult.AlreadyAccepted =>
                    Conflict("Trip has already been accepted"),

                AcceptTripResult.NoAvailableDriver =>
                    UnprocessableEntity("No available driver available"),

                AcceptTripResult.TripStatusNotUpdated =>
                    Conflict("Trip status could not be updated"),

                AcceptTripResult.AssigningDriverFailed =>
                    StatusCode(StatusCodes.Status500InternalServerError,
                               "Failed to assign driver"),

                _ =>
                    StatusCode(StatusCodes.Status500InternalServerError,
                               "Unknown error occurred")
            };
        }



        [HttpPost("{tripId}/arrived")]
        public async Task<IActionResult> Arrived(int tripId)
        {
            var success = await _tripService.UpdateTripStatus(tripId, TripStatus.Arrived);
            return success ? Ok() : BadRequest();
        }

        [HttpPost("{tripId}/start")]
        public async Task<IActionResult> StartTrip(int tripId)
        {
            var success = await _tripService.UpdateTripStatus(tripId, TripStatus.InProgress);
            return success ? Ok() : BadRequest();
        }

        [HttpPost("{tripId}/complete")]
        public async Task<IActionResult> CompleteTrip(int tripId)
        {
            var success = await _tripService.CompleteTrip(tripId);
            return success ? Ok() : BadRequest();
        }


       
    }
}
