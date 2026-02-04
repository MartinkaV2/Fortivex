namespace FortivexAPI.Models
{
    public class PlayerStat
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int EnemiesKilled { get; set; }
        public int TimePlayed { get; set; }

        public Account Account { get; set; }
    }
}
