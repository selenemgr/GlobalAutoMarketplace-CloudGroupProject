namespace GlobalAutoAPI.DTO
{
    public class BrandDto
    {
        public int BrandId { get; set; }
        public string BName { get; set; } = string.Empty;
        public int NumberOfCars { get { return Cars.Count; } }
        public ICollection<CarWithoutDetailsDto> Cars { get; set; } = new List<CarWithoutDetailsDto>();
    }
}