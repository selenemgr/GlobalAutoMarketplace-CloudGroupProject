using GlobalAutoAPI.DTO;

namespace GlobalAutoAPI.DTO
{
    public class CarDto
    {
        public int CarId { get; set; }
        public BrandWithoutCarsDto Brand { get; set; } = new BrandWithoutCarsDto();
        public VehicleTypeWithoutCarsDto VehicleType { get; set; } = new VehicleTypeWithoutCarsDto();
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string? Color { get; set; }
        public string VIN { get; set; } = string.Empty;
    }
}