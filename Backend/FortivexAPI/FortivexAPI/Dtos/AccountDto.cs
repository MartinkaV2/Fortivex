namespace FortivexAPI.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string PasswordHash { get; internal set; }
    }
}
