using GlobalAutoMarketplaceFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GlobalAutoMarketplaceFrontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("GlobalAutoApi");
        }

        public async Task<IActionResult> IndexAsync()
        {
            var carResponse = await _httpClient.GetAsync("cars");

            if (!carResponse.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var cars = await carResponse.Content.ReadFromJsonAsync<IEnumerable<Car>>();

            var carCards = cars.Select(car =>
            {
                return new CarCardViewModel
                {
                    BrandName = car.BrandName ?? "Unknown Brand",
                    TypeName = car.TypeName ?? "Unknown Type",
                    Model = car.Model,
                    Year = car.Year,
                    Price = car.Price,
                    Color = car.Color,
                };
            });

            var viewModel = new HomeViewModel
            {
                Cars = carCards
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
