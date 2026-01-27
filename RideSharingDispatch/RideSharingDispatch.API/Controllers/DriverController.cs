using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSharingDispatch.Application.DTOs;
using RideSharingDispatch.Application.Interfaces;

namespace RideSharingDispatch.API.Controllers
{
    [Authorize(Roles = "Driver")]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly ITripService _tripService;

        public DriverController(IDriverService driverService, ITripService tripService)
        {
            this._driverService = driverService;
            _tripService = tripService;
        }

        [HttpPut("{driverId}/status")]
        public async Task<IActionResult> UpdateStatus(int driverId, UpdateDriverStatusRequest request)
        {
            bool result = await _driverService.SetOnlineStatus(request.IsOnline, driverId);

            if (result == false)
            {
                return BadRequest("Status Change failed");
            }
            
            return Ok();
        }

        [HttpPut("{driverId}/location")]
        public async Task<IActionResult> UpdateLocation(int driverId, UpdateDriverLocationRequest request)
        {
            bool result = await _driverService.UpdateLocation(
                request.Latitude,
                request.Longitude,
                driverId
            );

            if (result == false)
            {
                return BadRequest("Location Change failed");
            }

            return Ok(result);
        }



    }
}
