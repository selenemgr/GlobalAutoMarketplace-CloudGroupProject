namespace GlobalAutoAPI.DTO
{
    public class VehicleTypeWithoutCarsDto
    {
        public int VehicleTypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}