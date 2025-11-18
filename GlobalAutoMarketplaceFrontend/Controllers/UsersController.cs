using GlobalAutoLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAutoMarketplaceFrontend.Controllers
{
    public class UsersController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsersController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GlobalAutoApi");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("users");
            if (!response.IsSuccessStatusCode) return View("Error");

            var users = await response.Content.ReadFromJsonAsync<IEnumerable<User>>();
            return View(users);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"users/{id}");
            if (!response.IsSuccessStatusCode) return View("Error");

            var user = await response.Content.ReadFromJsonAsync<User>();
            return View(user);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var response = await _httpClient.PostAsJsonAsync("users", user);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"users/{id}");
            if (!response.IsSuccessStatusCode) return View("Error");

            var user = await response.Content.ReadFromJsonAsync<User>();
            return View(user);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, User user)
        {
            var response = await _httpClient.PutAsJsonAsync($"users/{id}", user);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"users/{id}");
            if (!response.IsSuccessStatusCode) return View("Error");

            var user = await response.Content.ReadFromJsonAsync<User>();
            return View(user);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"users/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}
