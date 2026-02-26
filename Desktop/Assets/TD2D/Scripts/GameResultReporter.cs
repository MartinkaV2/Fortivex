using UnityEngine;
using System.Collections;

/// <summary>
/// Meccs végén menti a játék eredményét a recentgames ÉS a player_map_progress táblába.
/// Tedd ezt a scriptet a Level scene-ben egy GameObject-re (pl. LevelManager mellé).
/// </summary>
public class GameResultReporter : MonoBehaviour
{
    [Tooltip("A pálya neve, ahogy az adatbázisban szerepel (pl. Nyár, Tél, Ősz)")]
    public string mapName = "Nyár";

    [Tooltip("A pálya MapId-je az adatbázisban (Nyár=1, Tél=2, Ősz=3)")]
    public int mapId = 1;

    [Tooltip("Hány körből áll ez a pálya? (Tél=5, Nyár=7, Ősz=9)")]
    public int totalWaves = 7;

    private float gameStartTime;
    private int currentWave = 0;
    private int goldEarned  = 0;
    private bool resultSaved = false;

    void Awake()
    {
        gameStartTime = Time.time;
    }

    void OnEnable()
    {
        EventManager.StartListening("Victory",     OnVictory);
        EventManager.StartListening("Defeat",      OnDefeat);
        EventManager.StartListening("WaveStart",   OnWaveStart);
        EventManager.StartListening("GoldChanged", OnGoldChanged);
    }

    void OnDisable()
    {
        EventManager.StopListening("Victory",     OnVictory);
        EventManager.StopListening("Defeat",      OnDefeat);
        EventManager.StopListening("WaveStart",   OnWaveStart);
        EventManager.StopListening("GoldChanged", OnGoldChanged);
    }

    private void OnWaveStart(GameObject obj, string param)
    {
        if (int.TryParse(param, out int waveIndex))
        {
            currentWave = waveIndex + 1;
            Debug.Log($"🌊 WaveStart event → currentWave: {currentWave}");
        }
    }

    private void OnGoldChanged(GameObject obj, string param)
    {
        if (int.TryParse(param, out int gold))
            goldEarned = gold;
    }

    private void OnVictory(GameObject obj, string param)
    {
        if (resultSaved) return;
        resultSaved = true;
        StartCoroutine(ReportGame(won: true));
    }

    private void OnDefeat(GameObject obj, string param)
    {
        if (resultSaved) return;
        resultSaved = true;
        StartCoroutine(ReportGame(won: false));
    }

    private IEnumerator ReportGame(bool won)
    {
        int accountId = PlayerPrefs.GetInt("accountId", 0);
        int duration  = Mathf.RoundToInt(Time.time - gameStartTime);

        int kills = 0;
        if (PlayerStatsManager.Instance != null)
            kills = PlayerStatsManager.Instance.enemiesKilled;

        if (accountId == 0)
        {
            Debug.LogWarning("⚠️ GameResultReporter: accountId = 0, nincs bejelentkezve! Nem mentünk.");
            yield break;
        }

        if (APIManager.Instance == null)
        {
            Debug.LogError("❌ GameResultReporter: APIManager nincs jelen!");
            yield break;
        }

        // Győzelemnél az összes kört teljesítettnek vesszük
        int wavesCompleted = won ? totalWaves : currentWave;

        // Hány % kör lett teljesítve
        int completionPercent = totalWaves > 0
            ? Mathf.Clamp(Mathf.RoundToInt((float)wavesCompleted / totalWaves * 100f), 0, 100)
            : 0;

        // Csillag logika:
        // 3 ⭐ = győzelem (100%)
        // 2 ⭐ = elérte a 60%-ot (de nem nyert)
        // 1 ⭐ = elérte a 30%-ot
        // 0 ⭐ = kevesebb mint 30%
        int stars;
        if (won)
            stars = 3;
        else if (completionPercent >= 60)
            stars = 2;
        else if (completionPercent >= 30)
            stars = 1;
        else
            stars = 0;

        Debug.Log($"📊 Meccs vége → Won:{won} | Map:{mapName}(Id:{mapId}) | Wave:{wavesCompleted}/{totalWaves} | {completionPercent}% | ⭐{stars} | Duration:{duration}s | Kills:{kills} | Gold:{goldEarned}");

        // 1) RecentGames mentése
        yield return StartCoroutine(APIManager.Instance.SaveRecentGame(
            accountId:     accountId,
            mapName:       mapName,
            won:           won,
            waveReached:   currentWave,
            duration:      duration,
            goldEarned:    goldEarned,
            enemiesKilled: kills,
            xpEarned:      0
        ));

        // 2) MapProgress mentése
        yield return StartCoroutine(APIManager.Instance.SaveMapProgress(
            accountId:         accountId,
            mapId:             mapId,
            mapName:           mapName,
            completed:         won,
            stars:             stars,
            timeSeconds:       duration,
            completionPercent: completionPercent
        ));
    }
}