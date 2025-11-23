using GlobalAutoMarketplaceFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace GlobalAutoMarketplaceFrontend.Controllers
{
    public class CarsController : Controller
    {
        private readonly HttpClient _httpClient;

        public CarsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GlobalAutoApi");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Fetch car types
            var typesResponse = await _httpClient.GetAsync("types");
            var types = await typesResponse.Content.ReadFromJsonAsync<IEnumerable<CarType>>();
            ViewBag.CarTypes = types;

            // Fetch brands
            var brandsResponse = await _httpClient.GetAsync("brands");
            var brands = await brandsResponse.Content.ReadFromJsonAsync<IEnumerable<Brand>>();
            ViewBag.Brands = brands;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarCreateDto car)
        {
            car.VIN = GenerateVin();
            Console.WriteLine(car);
            // Fetch car types
            var typesResponse = await _httpClient.GetAsync("types");
            var types = await typesResponse.Content.ReadFromJsonAsync<IEnumerable<CarType>>();
            ViewBag.CarTypes = types;

            // Fetch brands
            var brandsResponse = await _httpClient.GetAsync("brands");
            var brands = await brandsResponse.Content.ReadFromJsonAsync<IEnumerable<Brand>>();
            ViewBag.Brands = brands;

            var response = await _httpClient.PostAsJsonAsync("cars", car);

            Console.WriteLine(response);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Failed to create the car. Please try again.");
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

        [HttpPut]
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

        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"cars/{id}");
            return RedirectToAction(nameof(Index));
        }

        private string GenerateVin()
        {
            const string chars = "ABCDEFGHJKLMNPRSTUVWXYZ0123456789";
            var random = new Random();

            return new string(Enumerable.Range(0, 17)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }
    }
}


