using Microsoft.AspNetCore.Mvc;

namespace GlobalAutoMarketplaceFrontend.Controllers
{
    public class BrandsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
