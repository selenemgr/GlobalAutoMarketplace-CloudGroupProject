using System.ComponentModel.DataAnnotations;

namespace GlobalAutoAPI.DTO
{

    // For manipulation means: for update and changing if user want to change each fields that is why we have the error messages to show what is wrong if user tried to put something wrong 
    public class CarForManipulationDto
    {
        [Required(ErrorMessage = "You must provide a BrandId.")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "You must provide a VehicleTypeId.")]
        public int VehicleTypeId { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        [MaxLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Year is required.")]
        [Range(1900, 2025)]
        public int Year { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 1000000.00)]
        public decimal Price { get; set; }

        [MaxLength(30)]
        public string? Color { get; set; }

        [Required(ErrorMessage = "VIN is required.")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN must be 17 characters.")]
        public string VIN { get; set; } = string.Empty;
    }
}