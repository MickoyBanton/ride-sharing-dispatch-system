using Microsoft.AspNetCore.Mvc;

namespace RideSharingDispatch.API.Controllers
{
    public class TripController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
