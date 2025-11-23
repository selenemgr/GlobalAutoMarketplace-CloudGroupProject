using GlobalAutoMarketplaceFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GlobalAutoMarketplaceFrontend.ViewComponents
{
    public class TypeFilterViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TypeFilterViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient("GlobalAutoApi");
            var response = await client.GetAsync("types");

            var types = await response.Content.ReadFromJsonAsync<IEnumerable<CarType>>();

            var carTypes = types.Select(type => new CarType
            {
                VehicleTypeId = type.VehicleTypeId,
                TypeName = type.TypeName,
                Description = type.Description
            }).ToList();

            return View("Default", carTypes);
        }
    }
}
