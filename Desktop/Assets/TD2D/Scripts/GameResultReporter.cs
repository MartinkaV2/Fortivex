using UnityEngine;
using System.Collections;

/// <summary>
/// Meccs végén menti a játék eredményét a recentgames táblába.
/// Tedd ezt a scriptet a Level scene-ben egy GameObject-re (pl. LevelManager mellé).
/// </summary>
public class GameResultReporter : MonoBehaviour
{
    [Tooltip("A pálya neve, ahogy az adatbázisban szerepel (pl. Nyár, Tél, Ősz)")]
    public string mapName = "Nyár";

    private float gameStartTime;
    private int currentWave = 0;
    private int goldEarned  = 0;
    private bool resultSaved = false; // hogy ne mentse kétszer

    void Awake()
    {
        gameStartTime = Time.time;
    }

    void OnEnable()
    {
        EventManager.StartListening("Victory",    OnVictory);
        EventManager.StartListening("Defeat",     OnDefeat);
        EventManager.StartListening("WaveStart",  OnWaveStart);   // ✅ WavesTimer ezt tüzeli
        EventManager.StartListening("GoldChanged", OnGoldChanged);
    }

    void OnDisable()
    {
        EventManager.StopListening("Victory",    OnVictory);
        EventManager.StopListening("Defeat",     OnDefeat);
        EventManager.StopListening("WaveStart",  OnWaveStart);
        EventManager.StopListening("GoldChanged", OnGoldChanged);
    }

    // WavesTimer: WaveStart eventben a param 0-alapú index
    // pl. 1. hullám → "0", 2. hullám → "1" stb.
    // +1-et adunk, hogy 1-alapú legyen az adatbázisban
    private void OnWaveStart(GameObject obj, string param)
    {
        if (int.TryParse(param, out int waveIndex))
        {
            currentWave = waveIndex + 1;
            Debug.Log($"🌊 WaveStart event → currentWave: {currentWave}");
        }
    }

    // Ha az UiManager/GoldManager küld ilyen eventet, nyomon követjük az aranyat
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

        // PlayerStatsManager-ből vesszük az ellenség kill számot, ha van
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

        Debug.Log($"📊 Meccs vége → Won: {won} | Map: {mapName} | Wave: {currentWave} | Duration: {duration}s | Kills: {kills} | Gold: {goldEarned}");

        yield return StartCoroutine(APIManager.Instance.SaveRecentGame(
            accountId:    accountId,
            mapName:      mapName,
            won:          won,
            waveReached:  currentWave,
            duration:     duration,
            goldEarned:   goldEarned,
            enemiesKilled: kills,
            xpEarned:     0
        ));
    }
}