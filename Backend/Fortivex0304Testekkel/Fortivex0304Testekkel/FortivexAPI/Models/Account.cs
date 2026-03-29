namespace FortivexAPI.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }

        public Admin? Admin { get; set; }
        public PlayerProgress? PlayerProgress { get; set; }
        public PlayerStat? PlayerStat { get; set; } 
        public User? User { get; set; }
    }
}
