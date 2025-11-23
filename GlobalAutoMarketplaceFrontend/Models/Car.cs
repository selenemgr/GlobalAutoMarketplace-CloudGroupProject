namespace GlobalAutoMarketplaceFrontend.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int VehicleTypeId { get; set; }
        public string TypeName { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public string Color { get; set; } 
    }

    public class CarCreateDto
    {
        public int BrandId { get; set; }
        public int VehicleTypeId { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string VIN { get; set; } = string.Empty;
    }
}
