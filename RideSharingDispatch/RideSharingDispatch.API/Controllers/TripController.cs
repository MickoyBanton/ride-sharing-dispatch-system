using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSharingDispatch.Application.DTOs;
using RideSharingDispatch.Application.Interfaces;
using RideSharingDispatch.Domain.Entities;
using RideSharingDispatch.Domain.Enums;

namespace RideSharingDispatch.API.Controllers
{
    [Authorize]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly IDriverService _driverService;

        public TripController(ITripService tripService, IDriverService driverService)
        {
            _tripService = tripService;
            _driverService = driverService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrip(CreateTripRequest request)
        {
            var trip = new Trip
            {
                RiderId = request.RiderId,
                PickupLatitude = request.PickupLatitude,
                PickupLongitude = request.PickupLongitude,
                DestinationLatitude = request.DestinationLatitude,
                DestinationLongitude = request.DestinationLongitude,
                VehicleType = request.VehicleType,
                TripStatus = TripStatus.Requested
            };

            await _tripService.CreateTrip(trip);

            return Ok(trip);
        }

        [HttpGet("{tripId}")]
        public async Task<IActionResult> GetTrip(int tripId)
        {
            var trip = await _tripService.GetTrip(tripId);

            if (trip == null)
                return NotFound();

            return Ok(trip);
        }

        [HttpPost("{tripId}/cancel")]
        public async Task<IActionResult> CancelTrip(int tripId)
        {
            var result = await _tripService.CancelTrip(tripId);
            return Ok("Trip cancelled");
        }

        [HttpPost("trips/{tripId}/accept")]
        public async Task<IActionResult> AcceptTrip(int tripId)
        {
            bool result = await _tripService.AssignDriver(tripId);
            if (result == false)
            {
                return BadRequest();
            }
            return Ok("Trip accepted");
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
