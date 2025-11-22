namespace GlobalAutoAPI.DTO
{
    public class CarWithoutDetailsDto
    {
        public int CarId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string? Color { get; set; }
    }
}