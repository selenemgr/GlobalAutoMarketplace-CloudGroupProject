using GlobalAutoLibrary.Models;

namespace GlobalAutoAPI.DTO
{
    public class VehicleTypeDto
    {
        public int VehicleTypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int NumberOfCars { get { return Cars.Count; } }
        public ICollection<CarWithoutDetailsDto> Cars { get; set; } = new List<CarWithoutDetailsDto>();
    }
}