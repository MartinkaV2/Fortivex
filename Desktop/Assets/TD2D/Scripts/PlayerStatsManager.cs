using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    // ===== PLAYER DATA =====
    private int myDbId;
    private int accountId;
    public int AccountId => accountId;

    public int enemiesKilled;
    public int timePlayedSeconds;

    // ===== SZERVEREN TÁROLT ÖSSZES MEZŐ (hogy PUT-nál ne nullázzuk vissza) =====
    private int cachedLevel        = 1;
    private int cachedCurrentXp    = 0;
    private int cachedNextLevelXp  = 100;
    private int cachedWins         = 0;
    private int cachedTotalGames   = 0;
    private long cachedTotalGold   = 0;
    private long cachedCurrentGold = 0;
    private int cachedMaxWave      = 0;

    // ===== PUBLIKUS OLVASHATÓ ÉRTÉKEK (AchievementManager számára) =====
    public long CachedTotalGold      => cachedTotalGold;
    public int  CachedMaxWaveReached => cachedMaxWave;

    private float timerBuffer    = 0f;
    private bool isTracking      = false;
    private bool autoSaveRunning = false;
    private bool isSaving        = false; // folyamatban lévő mentés jelzője
    private bool isLoading       = false; // GET fut → PUT blokkolva

    private const string BASE_URL = "https://fortivex.runasp.net/api/PlayerStats";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Kilépés előtt megvárjuk hogy a mentés lefusson
            Application.wantsToQuit += OnWantsToQuit;
        }
        else Destroy(gameObject);
    }

    void Update()
    {
        if (!isTracking) return;
        timerBuffer += Time.deltaTime;
        if (timerBuffer >= 1f)
        {
            timePlayedSeconds++;
            timerBuffer -= 1f;
        }
    }

    // =========================================================
    // LOGIN UTÁN HÍVJUK
    // =========================================================
    public void InitAndLoadStats(int accId)
    {
        accountId = accId;
        StartCoroutine(GetStatsAndStartTracking());
    }

    // =========================================================
    // STAT LETÖLTÉS
    // =========================================================
    IEnumerator GetStatsAndStartTracking()
    {
        isLoading = true; // ← BLOKKOLJUK A PUT-OT amíg a GET fut

        UnityWebRequest req = UnityWebRequest.Get(BASE_URL);
        req.SetRequestHeader("Authorization", "Bearer " + APIManager.Instance.Token);
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            isLoading = false;
            Debug.LogError("STATS LOAD ERROR: " + req.error);
            yield break;
        }

        PlayerStatsDto[] allStats = JsonHelper.FromJson<PlayerStatsDto>(req.downloadHandler.text);
        bool found = false;

        foreach (var stat in allStats)
        {
            if (stat.AccountId == accountId)
            {
                myDbId            = stat.Id;
                enemiesKilled     = stat.EnemiesKilled;
                timePlayedSeconds = stat.TimePlayed;

                // ===== ÖSSZES MEZŐ CACHE-ELÉSE =====
                cachedLevel        = stat.Level;
                cachedCurrentXp    = stat.CurrentXp;
                cachedNextLevelXp  = stat.NextLevelXp;
                cachedWins         = stat.Wins;
                cachedTotalGames   = stat.TotalGames;
                cachedTotalGold    = stat.TotalGold;
                cachedCurrentGold  = stat.CurrentGold;
                cachedMaxWave      = stat.MaxWaveReached;

                found = true;
                Debug.Log($"Stats betöltve → Time: {timePlayedSeconds}s | Kills: {enemiesKilled} | Level: {cachedLevel}");
                break;
            }
        }

        isLoading = false; // ← GET kész, PUT újra engedélyezett

        if (!found)
        {
            Debug.Log("Nincs stat rekord → létrehozás...");
            yield return StartCoroutine(CreateStatsOnServer());
            yield break;
        }

        StartTracking();
    }

    // =========================================================
    // STAT CREATE HA NINCS
    // =========================================================
    IEnumerator CreateStatsOnServer()
    {
        PlayerStatsDto dto = new PlayerStatsDto
        {
            AccountId      = accountId,
            EnemiesKilled  = 0,
            TimePlayed     = 0,
            Level          = 1,
            CurrentXp      = 0,
            NextLevelXp    = 100,
            Wins           = 0,
            TotalGames     = 0,
            TotalGold      = 0,
            CurrentGold    = 0,
            MaxWaveReached = 0
        };

        string json  = JsonUtility.ToJson(dto);
        byte[] body  = Encoding.UTF8.GetBytes(json);

        UnityWebRequest req = new UnityWebRequest(BASE_URL, "POST");
        req.uploadHandler   = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type",  "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + APIManager.Instance.Token);
        yield return req.SendWebRequest();

        string responseBody = req.downloadHandler != null ? req.downloadHandler.text : "(no body)";
        Debug.Log($"CREATE STATS RESPONSE ({req.responseCode}): {responseBody}");

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"CREATE STATS ERROR: {req.error} | HTTP {req.responseCode} | Body: {responseBody}");
            yield break;
        }

        Debug.Log("Stats rekord létrehozva!");
        yield return StartCoroutine(GetStatsAndStartTracking());
    }

    // =========================================================
    // TRACKING INDÍTÁS
    // =========================================================
    void StartTracking()
    {
        isTracking = true;
        if (!autoSaveRunning)
            StartCoroutine(AutoSaveRoutine());
    }

    // =========================================================
    // AUTOSAVE 60 mp
    // =========================================================
    IEnumerator AutoSaveRoutine()
    {
        autoSaveRunning = true;
        while (isTracking)
        {
            yield return new WaitForSeconds(60f);
            yield return StartCoroutine(PutStats());
        }
        autoSaveRunning = false;
    }

    // =========================================================
    // KILL HOZZÁADÁS
    // =========================================================
    public void AddKill()
    {
        enemiesKilled++;
        StartCoroutine(PutStats());
    }

    // =========================================================
    // SAVE ENTRY (külső híváshoz)
    // =========================================================
    public void SaveStats()
    {
        if (!isTracking || myDbId == 0) return;
        StartCoroutine(PutStats());
    }

    // =========================================================
    // MECCS EREDMÉNY ALKALMAZÁSA
    // Győzelem után a GameResultReporter hívja meg,
    // hogy a cache naprakész legyen az autosave előtt.
    // =========================================================
    public void ApplyGameResult(int goldEarned, int xpEarned)
    {
        cachedTotalGold   += goldEarned;
        cachedCurrentGold += goldEarned;

        cachedCurrentXp += xpEarned;
        while (cachedCurrentXp >= cachedNextLevelXp)
        {
            cachedCurrentXp  -= cachedNextLevelXp;
            cachedLevel++;
            cachedNextLevelXp = cachedLevel * 100;
            Debug.Log($"🎉 LEVEL UP! → Level {cachedLevel} | NextLevelXp: {cachedNextLevelXp}");
        }

        Debug.Log($"✅ ApplyGameResult: Gold +{goldEarned} (total:{cachedTotalGold}) | " +
                  $"XP +{xpEarned} → CurrentXp:{cachedCurrentXp}/{cachedNextLevelXp} | Lv:{cachedLevel}");

        StartCoroutine(PutStats());
    }

    // =========================================================
    // PUT UPDATE — MINDEN MEZŐT ELKÜLDI
    // =========================================================
    IEnumerator PutStats()
    {
        // Ha még töltünk a szerverről, NE írjuk felül 0-val!
        if (myDbId == 0 || !isTracking || isLoading) yield break;

        isSaving = true;

        PlayerStatsDto dto = new PlayerStatsDto
        {
            Id             = myDbId,
            AccountId      = accountId,
            EnemiesKilled  = enemiesKilled,
            TimePlayed     = timePlayedSeconds,
            // ===== CACHE-ELT ÉRTÉKEK - nem nullázzuk vissza =====
            Level          = cachedLevel,
            CurrentXp      = cachedCurrentXp,
            NextLevelXp    = cachedNextLevelXp,
            Wins           = cachedWins,
            TotalGames     = cachedTotalGames,
            TotalGold      = cachedTotalGold,
            CurrentGold    = cachedCurrentGold,
            MaxWaveReached = cachedMaxWave
        };

        string json = JsonUtility.ToJson(dto);
        byte[] body = Encoding.UTF8.GetBytes(json);
        string url  = $"{BASE_URL}/{myDbId}";

        UnityWebRequest req = new UnityWebRequest(url, "PUT");
        req.uploadHandler   = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type",  "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + APIManager.Instance.Token);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            Debug.LogError("STATS SAVE ERROR: " + req.error + " | " + req.downloadHandler.text);
        else
            Debug.Log($"STATS SAVED → Kills: {enemiesKilled} | Time: {timePlayedSeconds}s");

        isSaving = false;
    }

    // =========================================================
    // KILÉPÉS ELŐTTI MENTÉS
    // Coroutine helyett várakozás loop-pal, mert OnApplicationQuit-ban
    // nem fut coroutine
    // =========================================================
    private bool OnWantsToQuit()
    {
        if (myDbId == 0 || !isTracking) return true;

        // Ha már folyamatban van egy mentés, várjuk meg
        if (isSaving) return false;

        StartCoroutine(SaveAndQuit());
        return false; // Ne lépjen ki azonnal
    }

    IEnumerator SaveAndQuit()
    {
        isSaving = true;
        yield return StartCoroutine(PutStats());
        Application.Quit();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) StartCoroutine(PutStats());
    }
}