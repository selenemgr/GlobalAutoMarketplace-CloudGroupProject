using GlobalAutoMarketplaceFrontend.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

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
            var brandsResponse = await _httpClient.GetAsync("brands");

            if (!brandsResponse.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var brands = await brandsResponse.Content.ReadFromJsonAsync<IEnumerable<Brand>>();

            ViewBag.Brands = brands;

            var carResponse = await _httpClient.GetAsync("cars");

            if (!carResponse.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var cars = await carResponse.Content.ReadFromJsonAsync<IEnumerable<CarDetails>>();

            var carCards = cars.Select(car =>
            {
                return new CarCardViewModel
                {
                    CarId = car.CarId,
                    BrandName = car.Brand.Bname ?? "Unknown Brand",
                    TypeName = car.VehicleType.TypeName ?? "Unknown Type",
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

        [HttpPost]
        public async Task<IActionResult> Create(BrandCreateDto brand)
        {
            var response = await _httpClient.PostAsJsonAsync("brands", brand);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Brand created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create brand.";
            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> EditPatch(int brandId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                TempData["ErrorMessage"] = "Brand name is required.";
                return RedirectToAction("Index");
            }

            var carsResponse = await _httpClient.GetAsync($"brands/{brandId}?includeCars=true");

            if (!carsResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to upload cars with new Brand Name.";
                return RedirectToAction("Index");
            }

            var brandData = await carsResponse.Content.ReadAsStringAsync();
            var brandObj = JsonSerializer.Deserialize<BrandWithCarsDto>(brandData,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (brandObj?.Cars != null || brandObj.Cars.Count != 0)
            {
                foreach (var car in brandObj.Cars)
                {
                    var carPatch = new JsonPatchDocument();
                    carPatch.Replace("/brandName", newName);

                    var carJson = JsonSerializer.Serialize(carPatch.Operations);
                    var carContent = new StringContent(carJson, Encoding.UTF8, "application/json-patch+json");

                    await _httpClient.PatchAsync($"cars/{car.CarId}", carContent);
                }
            }

            foreach (var car in brandObj.Cars)
            {
                var carPatch = new JsonPatchDocument();
                carPatch.Replace("/brandName", newName);

                var carJson = JsonSerializer.Serialize(carPatch.Operations);
                var carContent = new StringContent(carJson, Encoding.UTF8, "application/json-patch+json");

                await _httpClient.PatchAsync($"cars/{car.CarId}", carContent);
            }

            var brandPatch = new JsonPatchDocument();
            brandPatch.Replace("/bName", newName);

            var brandJson = JsonSerializer.Serialize(brandPatch.Operations);
            var brandContent = new StringContent(brandJson, Encoding.UTF8, "application/json-patch+json");
            var brandResponse = await _httpClient.PatchAsync($"brands/{brandId}", brandContent);

            if (!brandResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to update brand.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Brand and all car names updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditPut(int brandId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                TempData["ErrorMessage"] = "Brand name is required.";
                return RedirectToAction("Index");
            }

            var brandUpdate = new BrandCreateDto
            {
                Bname = newName
            };

            var brandContent = new StringContent(JsonSerializer.Serialize(brandUpdate), Encoding.UTF8, "application/json");
            var brandResponse = await _httpClient.PutAsync($"brands/{brandId}", brandContent);
            if (brandResponse.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Brand updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update brand.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int brandId)
        {
            var response = await _httpClient.GetAsync($"brands/{brandId}?includeCars=true");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Unable to load brand information.";
                return RedirectToAction("Index");
            }

            var brand = await response.Content.ReadFromJsonAsync<BrandWithCarsDto>();
            if (brand == null)
            {
                TempData["ErrorMessage"] = "Brand not found.";
                return RedirectToAction("Index");
            }

            if (brand.Cars != null && brand.Cars.Count > 0)
            {
                foreach (var car in brand.Cars)
                {
                    var carDelete = await _httpClient.DeleteAsync($"cars/{car.CarId}");
                    if (!carDelete.IsSuccessStatusCode)
                    {
                        TempData["ErrorMessage"] = $"Failed to delete car ID {car.CarId}.";
                        return RedirectToAction("Index");
                    }
                }
            }

            var deleteBrandResponse = await _httpClient.DeleteAsync($"brands/{brandId}");

            if (deleteBrandResponse.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Brand and all associated cars deleted successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete brand.";
                return RedirectToAction("Index");
            }
        }
    }
}
