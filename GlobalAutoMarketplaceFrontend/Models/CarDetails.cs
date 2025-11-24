namespace GlobalAutoMarketplaceFrontend.Models
{
    public class CarDetails
    {
        public int CarId { get; set; }
        public Brand Brand { get; set; }
        public VehicleType VehicleType { get; set; }
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string? Color { get; set; }
        public string VIN { get; set; } = string.Empty;
    }
}
