using System.ComponentModel.DataAnnotations;

namespace GlobalAutoAPI.DTO
{
    // For manipulation means: for update and changing if user want to change each fields that is why we have the error messages to show what is wrong if user tried to put something wrong 
    public class VehicleTypeForManipulationDto
    {
        [Required(ErrorMessage = "Type Name is required.")]
        [MaxLength(50)]
        public string TypeName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}