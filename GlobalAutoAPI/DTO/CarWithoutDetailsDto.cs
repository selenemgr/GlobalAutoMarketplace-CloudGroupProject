namespace GlobalAutoAPI.DTO
{
    public class CarWithoutDetailsDto
    {
        public int CarId { get; set; }
        public string BrandBName { get; set; } = string.Empty; // note for mapping: that it is BrandBName there is a "B" before Name becuase mapping will look for Brand.BName
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal Price { get; set; }
    }
}