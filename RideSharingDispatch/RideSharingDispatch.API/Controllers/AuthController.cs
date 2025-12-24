using Microsoft.AspNetCore.Mvc;

namespace RideSharingDispatch.API.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
