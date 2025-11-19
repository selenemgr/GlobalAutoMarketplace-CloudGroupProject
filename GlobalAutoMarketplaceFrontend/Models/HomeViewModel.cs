namespace GlobalAutoMarketplaceFrontend.Models
{
    public class HomeViewModel
    {
        public IEnumerable<CarCardViewModel> Cars { get; set; } = new List<CarCardViewModel>();
    }

    public class CarCardViewModel
    {
        public int CarId { get; set; }
        public string BrandName { get; set; }
        public string SellerUsername { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public string VIN { get; set; }
    }
}
