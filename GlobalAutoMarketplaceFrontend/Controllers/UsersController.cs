using GlobalAutoLibrary.Models;
using GlobalAutoMarketplaceFrontend.Models;
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

        #region Login and Registration

        [HttpGet]
        public IActionResult Login() => PartialView("_LoginModal");

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_LoginModal", model);

            var response = await _httpClient.GetAsync($"users?email={model.Email}&password={model.Password}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return PartialView("_LoginModal", model);
            }

            var user = await response.Content.ReadFromJsonAsync<User>();

            // Store user info in session
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("UserRole", user.UserRole);

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult Register() => PartialView("_RegisterModal");

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("_RegisterModal", model);

            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                UserRole = "Buyer"
            };

            var response = await _httpClient.PostAsJsonAsync("users", newUser);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Registration failed.");
                return PartialView("_RegisterModal", model);
            }

            return Json(new { success = true });
        }

        #endregion

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
