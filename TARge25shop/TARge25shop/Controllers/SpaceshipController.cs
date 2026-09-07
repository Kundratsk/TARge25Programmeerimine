using Microsoft.AspNetCore.Mvc;

namespace TARge25shop.Controllers
{
    public class SpaceshipController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
