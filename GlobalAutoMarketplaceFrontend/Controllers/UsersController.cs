using Microsoft.AspNetCore.Mvc;

namespace GlobalAutoMarketplaceFrontend.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
