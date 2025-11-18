using System.ComponentModel.DataAnnotations;

namespace GlobalAutoAPI.DTO
{
    //check CarForManipulationDto top comments for Manipulation meaning 
    public class UserForManipulationDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Role is required.")]
        public string UserRole { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}