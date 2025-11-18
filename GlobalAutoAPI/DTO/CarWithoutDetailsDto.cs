namespace GlobalAutoAPI.DTO
{
    public class CarWithoutDetailsDto
    {
        public int CarId { get; set; }
        public string BrandBrandName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
    }
}