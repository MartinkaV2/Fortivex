using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    // ===== PLAYER DATA =====
    private int myDbId; // PlayerStats PK
    private int accountId;

    public int enemiesKilled;
    public int timePlayedSeconds;

    private float timerBuffer = 0f;
    private bool isTracking = false;
    private bool autoSaveRunning = false;

    private const string BASE_URL = "https://fortivex.runasp.net/api/PlayerStats";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        UnityWebRequest req = UnityWebRequest.Get(BASE_URL);
        req.SetRequestHeader("Authorization", "Bearer " + APIManager.Instance.Token);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("STATS LOAD ERROR: " + req.error);
            yield break;
        }

        string json = req.downloadHandler.text;
        PlayerStatsDto[] allStats = JsonHelper.FromJson<PlayerStatsDto>(json);

        bool found = false;

        foreach (var stat in allStats)
        {
            if (stat.accountId == accountId)
            {
                myDbId = stat.id;
                enemiesKilled = stat.enemiesKilled;
                timePlayedSeconds = stat.timePlayed;

                found = true;

                Debug.Log($"Stats betöltve → Time: {timePlayedSeconds}s | Kills: {enemiesKilled}");
                break;
            }
        }

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
            accountId = accountId,
            enemiesKilled = 0,
            timePlayed = 0
        };

        string json = JsonUtility.ToJson(dto);
        byte[] body = Encoding.UTF8.GetBytes(json);

        UnityWebRequest req = new UnityWebRequest(BASE_URL, "POST");
        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + APIManager.Instance.Token);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("CREATE STATS ERROR: " + req.error);
            yield break;
        }

        Debug.Log("Stats rekord létrehozva!");

        // újra lekérjük hogy megkapjuk ID-t
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
            SaveStats();
        }

        autoSaveRunning = false;
    }

    // =========================================================
    // KILL HOZZÁADÁS
    // =========================================================
    public void AddKill()
    {
        enemiesKilled++;
        SaveStats();
    }

    // =========================================================
    // SAVE ENTRY
    // =========================================================
    public void SaveStats()
    {
        if (!isTracking || myDbId == 0) return;

        StartCoroutine(PutStats());
    }

    // =========================================================
    // PUT UPDATE
    // =========================================================
    IEnumerator PutStats()
    {
        PlayerStatsDto dto = new PlayerStatsDto
        {
            id = myDbId,
            accountId = accountId,
            enemiesKilled = enemiesKilled,
            timePlayed = timePlayedSeconds
        };

        string json = JsonUtility.ToJson(dto);
        byte[] body = Encoding.UTF8.GetBytes(json);

        string url = $"{BASE_URL}/{myDbId}";

        UnityWebRequest req = new UnityWebRequest(url, "PUT");
        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + APIManager.Instance.Token);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("STATS SAVE ERROR: " + req.error + " | " + req.downloadHandler.text);
        }
        else
        {
            Debug.Log($"STATS SAVED → Time: {timePlayedSeconds}s");
        }
    }

    // =========================================================
    // PAUSE SAVE
    // =========================================================
    void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveStats();
    }

    // =========================================================
    // QUIT SAVE
    // =========================================================
    void OnApplicationQuit()
    {
        SaveStats();
    }
}
