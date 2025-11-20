using System.ComponentModel.DataAnnotations;

namespace GlobalAutoAPI.DTO
{
    //check CarForManipulationDto top comments for Manipulation meaning 
    public class BrandForManipulationDto
    {

        [Required(ErrorMessage = "Brand Name is required.")]
        [MaxLength(50)]
        public string BName { get; set; } = string.Empty;
    }
}