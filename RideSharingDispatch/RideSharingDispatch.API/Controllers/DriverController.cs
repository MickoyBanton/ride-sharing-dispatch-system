using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideSharingDispatch.Application.DTOs;
using RideSharingDispatch.Application.Interfaces;
using System.Security.Claims;

namespace RideSharingDispatch.API.Controllers
{
    [Authorize(Roles = "Driver")]
    [Route("api/drivers")]

    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly ITripService _tripService;

        public DriverController(IDriverService driverService, ITripService tripService)
        {
            this._driverService = driverService;
            _tripService = tripService;
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateDriverStatusRequest request)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool result = await _driverService.SetOnlineStatus(request.IsOnline, userId);

            if (result == false)
            {
                return BadRequest("Status Change failed");
            }

            return Ok(result);
        }

        [HttpPut("location")]
        public async Task<IActionResult> UpdateLocation([FromBody] UpdateDriverLocationRequest request)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool result = await _driverService.UpdateLocation(
                request.Latitude,
                request.Longitude,
                userId
            );

            if (result == false)
            {
                return BadRequest("Location Change failed");
            }

            return Ok(result);
        }



    }
}
