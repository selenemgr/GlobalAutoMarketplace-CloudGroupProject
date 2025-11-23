namespace GlobalAutoMarketplaceFrontend.Models
{
    public class HomeViewModel
    {
        public IEnumerable<CarCardViewModel> Cars { get; set; } = new List<CarCardViewModel>();
    }
}
