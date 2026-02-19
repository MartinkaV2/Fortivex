using UnityEngine;

[System.Serializable]
public class PlayerStatsDto
{
    // PascalCase to match ASP.NET server model serialization
    public int Id;
    public int AccountId;
    public int EnemiesKilled;
    public int TimePlayed;
    public int Level = 1;
    public int CurrentXp = 0;
    public int NextLevelXp = 100;
    public int Wins = 0;
    public int TotalGames = 0;
    public long TotalGold = 0;
    public long CurrentGold = 0;
    public int MaxWaveReached = 0;
}