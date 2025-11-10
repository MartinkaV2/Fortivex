namespace FortivexAPI.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Role { get; set; } = "Admin";
        public DateTime AssignedAt { get; set; } = DateTime.Now;

        public Account Account { get; set; }
    }
}
