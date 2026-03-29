namespace FortivexAPI.Dtos
{
    public class AdminDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Role { get; set; } = "Admin";
    }
}
