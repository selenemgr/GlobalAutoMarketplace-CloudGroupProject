using GlobalAutoMarketplaceFrontend.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
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
        public async Task<IActionResult> Details(int carId)
        {
            var response = await _httpClient.GetAsync($"cars/{carId}?includeDetails=true");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Could not load car details.";
                return RedirectToAction("Index", "Home");
            }

            var car = await response.Content.ReadFromJsonAsync<CarDetails>();

            if (car == null)
            {
                TempData["ErrorMessage"] = "Car not found.";
                return RedirectToAction("Index", "Home");
            }

            // Fetch car types
            var typesResponse = await _httpClient.GetAsync("types");
            var types = await typesResponse.Content.ReadFromJsonAsync<IEnumerable<VehicleType>>();
            ViewBag.CarTypes = types;

            // Fetch brands
            var brandsResponse = await _httpClient.GetAsync("brands");
            var brands = await brandsResponse.Content.ReadFromJsonAsync<IEnumerable<Brand>>();
            ViewBag.Brands = brands;

            return View(car);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Fetch car types
            var typesResponse = await _httpClient.GetAsync("types");
            var types = await typesResponse.Content.ReadFromJsonAsync<IEnumerable<VehicleType>>();
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

            var typesResponse = await _httpClient.GetAsync("types");
            var types = await typesResponse.Content.ReadFromJsonAsync<IEnumerable<VehicleType>>();
            ViewBag.CarTypes = types;

            var brandsResponse = await _httpClient.GetAsync("brands");
            var brands = await brandsResponse.Content.ReadFromJsonAsync<IEnumerable<Brand>>();
            ViewBag.Brands = brands;

            var response = await _httpClient.PostAsJsonAsync("cars", car);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = $"Car created successfully!";
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "Failed to create the car. Please try again. ";
            return View(car);
        }

        [HttpPost]
        public async Task<IActionResult> PatchField(int carId, string fieldName, string newValue)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                TempData["ErrorMessage"] = "Field to update is required.";
                return RedirectToAction("Details", new { carId = carId });
            }

            var patchDoc = new JsonPatchDocument();

            switch (fieldName.ToLower())
            {
                case "brandid":
                    if (int.TryParse(newValue, out var brandId))
                        patchDoc.Replace("/brandId", brandId);
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid Brand.";
                        return RedirectToAction("Details", new { carId = carId });
                    }
                    break;
                case "vehicletypeid":
                    if (int.TryParse(newValue, out var vehicleTypeId))
                        patchDoc.Replace("/vehicleTypeId", vehicleTypeId);
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid Vehicle Type.";
                        return RedirectToAction("Details", new { carId = carId });
                    }
                    break;
                case "model":
                    patchDoc.Replace("/model", newValue);
                    break;
                case "color":
                    patchDoc.Replace("/color", newValue);
                    break;
                case "price":
                    if (decimal.TryParse(newValue, out var price))
                        patchDoc.Replace("/price", price);
                    else
                        TempData["ErrorMessage"] = "Invalid price.";
                    break;
                case "year":
                    if (int.TryParse(newValue, out var year))
                        patchDoc.Replace("/year", year);
                    else
                        TempData["ErrorMessage"] = "Invalid year.";
                    break;
                default:
                    TempData["ErrorMessage"] = "Field not recognized.";
                    return RedirectToAction("Details", new { carId = carId });
            }

            var json = JsonSerializer.Serialize(patchDoc.Operations);
            var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
            var response = await _httpClient.PatchAsync($"cars/{carId}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = $"{fieldName} updated successfully!";
                return RedirectToAction("Details", new { carId = carId });
            }
            else
            {
                var errorText = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Failed to update {fieldName}: {errorText}";
                return RedirectToAction("Details", new { carId = carId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFull(int carId, CarCreateDto updatedCar)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fix the validation errors.";
                return View(updatedCar);
            }

            var response = await _httpClient.PutAsJsonAsync($"cars/{carId}", updatedCar);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Car updated successfully!";
                return RedirectToAction("Details", new { carId = carId });
            }
            else
            {
                var errorText = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Failed to update car: {errorText}";
                return RedirectToAction("Details", new { carId = carId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int carId)
        {
            var response = await _httpClient.DeleteAsync($"cars/{carId}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to delete the car. Please try again.";
            }
            else
            {
                TempData["SuccessMessage"] = "Car deleted successfully.";
            }

            return RedirectToAction("Index", "Home");
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


