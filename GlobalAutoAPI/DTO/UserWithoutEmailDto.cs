namespace GlobalAutoAPI.DTO
{
    public class UserWithoutEmailDto
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;
    }
}