namespace FortivexAPI.Dtos
{
    public class UserLoginResponseDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";


    }
}
