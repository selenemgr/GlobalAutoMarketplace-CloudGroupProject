namespace GlobalAutoMarketplaceFrontend.Models
{
    public class BrandWithCarsDto
    {
        public int BrandId { get; set; }
        public string BName { get; set; } = string.Empty;
        public int NumberOfCars { get { return Cars.Count; } }
        public ICollection<CarCardViewModel> Cars { get; set; } = new List<CarCardViewModel>();
    }
}
