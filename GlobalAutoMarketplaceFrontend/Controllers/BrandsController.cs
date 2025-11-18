using GlobalAutoLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAutoMarketplaceFrontend.Controllers
{
    public class BrandsController : Controller
    {
        private readonly HttpClient _httpClient;

        public BrandsController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GlobalAutoApi");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("brands");
            if (!response.IsSuccessStatusCode) return View("Error");

            var brands = await response.Content.ReadFromJsonAsync<IEnumerable<Brand>>();
            return View(brands);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"brands/{id}?includeCars=true");
            if (!response.IsSuccessStatusCode) return View("Error");

            var brand = await response.Content.ReadFromJsonAsync<Brand>();
            return View(brand);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Brand brand)
        {
            var response = await _httpClient.PostAsJsonAsync("brands", brand);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(brand);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"brands/{id}");
            if (!response.IsSuccessStatusCode) return View("Error");

            var brand = await response.Content.ReadFromJsonAsync<Brand>();
            return View(brand);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, Brand brand)
        {
            var response = await _httpClient.PutAsJsonAsync($"brands/{id}", brand);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(brand);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"brands/{id}");
            if (!response.IsSuccessStatusCode) return View("Error");

            var brand = await response.Content.ReadFromJsonAsync<Brand>();
            return View(brand);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"brands/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
