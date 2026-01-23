using Microsoft.AspNetCore.Mvc;

namespace RideSharingDispatch.API.Controllers
{
    public class RiderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
