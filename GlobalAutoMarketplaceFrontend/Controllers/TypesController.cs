using GlobalAutoMarketplaceFrontend.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace GlobalAutoMarketplaceFrontend.Controllers
{
    public class TypesController : Controller
    {
        private readonly HttpClient _httpClient;

        public TypesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GlobalAutoApi");
        }

        public async Task<IActionResult> Index()
        {
            var typesResponse = await _httpClient.GetAsync("types");

            if (!typesResponse.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var types = await typesResponse.Content.ReadFromJsonAsync<IEnumerable<VehicleType>>();

            ViewBag.VehicleTypes = types;

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
        public async Task<IActionResult> Create(VehicleTypeCreateDto vehicleType)
        {
            var response = await _httpClient.PostAsJsonAsync("types", vehicleType);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Vehicle type created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create vehicle type.";
            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> EditPatch(int typeId, string fieldName, string newValue)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                TempData["ErrorMessage"] = "Field to update is required.";
                return RedirectToAction("Index");
            }

            var patchDoc = new JsonPatchDocument();

            switch (fieldName.ToLower())
            {
                case "typename":
                    patchDoc.Replace("/typeName", newValue);
                    break;
                case "description":
                    patchDoc.Replace("/description", newValue);
                    break;
                default:
                    TempData["ErrorMessage"] = "Field not recognized.";
                    return RedirectToAction("Index");
            }

            var json = JsonSerializer.Serialize(patchDoc.Operations);
            var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");

            var response = await _httpClient.PatchAsync($"types/{typeId}", content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to update vehicle type.";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Vehicle Type updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditPut(int typeId, string newTypeName, string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newTypeName))
            {
                TempData["ErrorMessage"] = "Vehicle type name is required.";
                return RedirectToAction("Index");
            }

            var vehicleTypeUpdate = new VehicleTypeCreateDto
            {
                TypeName = newTypeName,
                Description = newDescription
            };

            var vehicleTypeContent = new StringContent(JsonSerializer.Serialize(vehicleTypeUpdate), Encoding.UTF8, "application/json");
            var vehicleTypeResponse = await _httpClient.PutAsync($"types/{typeId}", vehicleTypeContent);
            if (vehicleTypeResponse.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Vehicle Type updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update vehicle type.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int vehicleTypeId)
        {
            var typeResponse = await _httpClient.GetAsync($"types/{vehicleTypeId}?includeCars=true");

            if (!typeResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Failed to load vehicle type details.";
                return RedirectToAction("Index");
            }

            var typeData = await typeResponse.Content.ReadFromJsonAsync<VehicleTypeWithCarsDto>();

            // Delete all cars belonging to this type
            if (typeData.Cars != null && typeData.Cars.Any())
            {
                foreach (var car in typeData.Cars)
                {
                    await _httpClient.DeleteAsync($"cars/{car.CarId}");
                }
            }

            var deleteTypeResponse = await _httpClient.DeleteAsync($"types/{vehicleTypeId}");

            if (deleteTypeResponse.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Vehicle type and all associated cars deleted successfully.";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Failed to delete vehicle type.";
            return RedirectToAction("Index");
        }
    }
}
