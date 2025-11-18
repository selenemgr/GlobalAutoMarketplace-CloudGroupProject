using Microsoft.AspNetCore.Mvc;
using GlobalAutoLibrary.Models;

namespace GlobalAutoMarketplaceFrontend.Controllers
{
    public class CarsController : Controller
    {
        private readonly HttpClient _httpClient;

        public CarsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GlobalAutoApi");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("cars");
            if (!response.IsSuccessStatusCode) return View("Error");

            var cars = await response.Content.ReadFromJsonAsync<IEnumerable<Car>>();
            return View(cars);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"cars/{id}?includeDetails=true");
            if (!response.IsSuccessStatusCode) return View("Error");

            var car = await response.Content.ReadFromJsonAsync<Car>();
            return View(car);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            var response = await _httpClient.PostAsJsonAsync("cars", car);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(car);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"cars/{id}?includeDetails=true");
            if (!response.IsSuccessStatusCode) return View("Error");

            var car = await response.Content.ReadFromJsonAsync<Car>();
            return View(car);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Car car)
        {
            var response = await _httpClient.PutAsJsonAsync($"cars/{id}", car);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(car);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"cars/{id}");
            if (!response.IsSuccessStatusCode) return View("Error");

            var car = await response.Content.ReadFromJsonAsync<Car>();
            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"cars/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}


