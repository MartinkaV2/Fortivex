namespace FortivexAPI.Models
{
    public class PlayerProgress
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int? BestTime { get; set; }

        public Account Account { get; set; }
    }
}
