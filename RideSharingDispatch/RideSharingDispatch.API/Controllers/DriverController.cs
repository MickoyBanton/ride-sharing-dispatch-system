using Microsoft.AspNetCore.Mvc;

namespace RideSharingDispatch.API.Controllers
{
    public class DriverController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
