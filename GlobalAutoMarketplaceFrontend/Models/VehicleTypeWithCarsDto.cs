namespace GlobalAutoMarketplaceFrontend.Models
{
    public class VehicleTypeWithCarsDto
    {
        public int VehicleTypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public int NumberOfCars { get { return Cars.Count; } }
        public ICollection<CarCardViewModel> Cars { get; set; } = new List<CarCardViewModel>();

    }
}
