using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;

    private string baseUrl = "https://fortivex.runasp.net/api";

    // 🔑 JWT TOKEN
    public string Token { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🔑 TOKEN BEÁLLÍTÁSA LOGIN UTÁN
    public void SetToken(string token)
    {
        Token = token;
        Debug.Log("JWT token elmentve");
    }

    // =========================
    // REGISZTRÁCIÓ
    // =========================
    public IEnumerator Register(
        string username,
        string email,
        string password,
        Action<string> onSuccess,
        Action<string> onError)
    {
        string url = baseUrl + "/Accounts/register";

        RegisterRequest requestData = new RegisterRequest
        {
            userName = username,
            email = email,
            passwordHash = password,
            role = "Player"
        };

        string jsonData = JsonUtility.ToJson(requestData);
        Debug.Log("Regisztráció küldése: " + jsonData);

        yield return StartCoroutine(
            PostRequest(url, jsonData, false, onSuccess, onError)
        );
    }

    // =========================
    // LOGIN
    // =========================
    public IEnumerator Login(
        string username,
        string password,
        Action<string> onSuccess,
        Action<string> onError)
    {
        string url = baseUrl + "/Accounts/login";

        LoginRequest requestData = new LoginRequest
        {
            userName = username,
            passwordHash = password
        };

        string jsonData = JsonUtility.ToJson(requestData);
        Debug.Log("Login küldése: " + jsonData);

        yield return StartCoroutine(
            PostRequest(url, jsonData, false, onSuccess, onError)
        );
    }

    // =========================
    // ÁLTALÁNOS POST (AUTH OPCIÓVAL)
    // =========================
    private IEnumerator PostRequest(
        string url,
        string jsonData,
        bool useAuth,
        Action<string> onSuccess,
        Action<string> onError)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            if (useAuth && !string.IsNullOrEmpty(Token))
            {
                request.SetRequestHeader(
                    "Authorization",
                    "Bearer " + Token
                );
            }

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string errorMsg = request.error;
                if (request.downloadHandler != null)
                    errorMsg += " | Backend válasz: " + request.downloadHandler.text;

                onError?.Invoke(errorMsg);
            }
            else
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
        }
    }

    // =========================================================
    // ================= GET ACCOUNTID BY USERNAME =============
    // =========================================================

    [Serializable]
    public class AccountDto
    {
        public int id;
        public string username;
        public string email;
        public string createdAt;
        public string lastLogin;
    }

    public IEnumerator GetAccountIdByUsername(
        string username,
        Action<int> onSuccess,
        Action<string> onError)
    {
        string url = baseUrl + "/Accounts";

        UnityWebRequest request = UnityWebRequest.Get(url);

        if (!string.IsNullOrEmpty(Token))
        {
            request.SetRequestHeader("Authorization", "Bearer " + Token);
        }

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            string errorMsg = request.error;
            if (request.downloadHandler != null)
                errorMsg += " | Backend válasz: " + request.downloadHandler.text;

            onError?.Invoke(errorMsg);
            yield break;
        }

        try
        {
            string json = request.downloadHandler.text;
            Debug.Log("🔍 Accounts API válasz: " + json);

            AccountDto[] accounts = JsonHelper.FromJson<AccountDto>(json);

            foreach (var account in accounts)
            {
                if (account.username.Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Log($"✅ AccountId megtalálva: {account.id} (username: {account.username})");
                    onSuccess?.Invoke(account.id);
                    yield break;
                }
            }

            onError?.Invoke($"Nem található account ezzel a username-mel: {username}");
        }
        catch (Exception e)
        {
            onError?.Invoke($"JSON feldolgozási hiba: {e.Message}");
        }
    }

    // =========================================================
    // ================= RECENT GAME MENTÉSE ===================
    // =========================================================

    [Serializable]
    public class GameResultRequest
    {
        public bool Won;
        public string MapName;
        public int WaveReached;
        public int Duration;
        public int GoldEarned;
        public int EnemiesKilled;
        public int XpEarned;
    }

    public IEnumerator SaveRecentGame(
        int accountId,
        string mapName,
        bool won,
        int waveReached,
        int duration,
        int goldEarned = 0,
        int enemiesKilled = 0,
        int xpEarned = 0,
        Action onSuccess = null,
        Action<string> onError = null)
    {
        // ✅ accountId az URL-ben van, nem a body-ban
        string url = $"{baseUrl}/PlayerStats/account/{accountId}/game";

        GameResultRequest dto = new GameResultRequest
        {
            Won          = won,
            MapName      = mapName,
            WaveReached  = waveReached,
            Duration     = duration,
            GoldEarned   = goldEarned,
            EnemiesKilled = enemiesKilled,
            XpEarned     = xpEarned
        };

        string json = JsonUtility.ToJson(dto);
        Debug.Log($"📤 SaveRecentGame küldése → URL: {url} | Body: {json}");

        yield return StartCoroutine(PostRequest(url, json, true,
            (response) => {
                Debug.Log("✅ RecentGame elmentve: " + response);
                onSuccess?.Invoke();
            },
            (error) => {
                Debug.LogError("❌ RecentGame mentési hiba: " + error);
                onError?.Invoke(error);
            }
        ));
    }

    // =========================================================
    // ================= PLAYER STATS (KILL REGISTER) ==========
    // =========================================================

    [Serializable]
    public class PlayerStatsDto
    {
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

    public void RegisterKill(int accountId)
    {
        StartCoroutine(RegisterKillCoroutine(accountId));
    }

    private IEnumerator RegisterKillCoroutine(int accountId)
    {
        string getUrl = $"{baseUrl}/PlayerStats";
        UnityWebRequest getReq = UnityWebRequest.Get(getUrl);
        getReq.SetRequestHeader("Authorization", "Bearer " + Token);

        yield return getReq.SendWebRequest();

        if (getReq.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Get stats error: " + getReq.error + " | " + getReq.downloadHandler.text);
            yield break;
        }

        PlayerStatsDto[] allStats = JsonHelper.FromJson<PlayerStatsDto>(getReq.downloadHandler.text);

        PlayerStatsDto myStats = null;

        foreach (var s in allStats)
        {
            if (s.AccountId == accountId)
            {
                myStats = s;
                break;
            }
        }

        if (myStats == null)
        {
            Debug.LogWarning($"⚠️ Nincs PlayerStats rekord az accountId={accountId}-hoz. Létrehozás...");

            yield return StartCoroutine(CreatePlayerStats(accountId, (newStats) =>
            {
                myStats = newStats;
            }));

            if (myStats == null)
            {
                Debug.LogError("❌ PlayerStats létrehozása sikertelen!");
                yield break;
            }
        }

        myStats.EnemiesKilled += 1;

        string putUrl = $"{baseUrl}/PlayerStats/{myStats.Id}";
        string json = JsonUtility.ToJson(myStats);

        UnityWebRequest putReq = new UnityWebRequest(putUrl, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        putReq.uploadHandler = new UploadHandlerRaw(bodyRaw);
        putReq.downloadHandler = new DownloadHandlerBuffer();
        putReq.SetRequestHeader("Content-Type", "application/json");
        putReq.SetRequestHeader("Authorization", "Bearer " + Token);

        yield return putReq.SendWebRequest();

        if (putReq.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Update stats error: " + putReq.error + " | " + putReq.downloadHandler.text);
        }
        else
        {
            Debug.Log("✅ Kill registered → enemiesKilled növelve");
            if (PlayerStatsManager.Instance != null)
            {
                PlayerStatsManager.Instance.enemiesKilled = myStats.EnemiesKilled;
                Debug.Log($"🔄 PSM szinkronizálva → enemiesKilled: {myStats.EnemiesKilled}");
            }
        }
    }

    // =========================================================
    // ================= MAP PROGRESS MENTÉSE ==================
    // =========================================================

    [Serializable]
    public class MapProgressUpdateRequest
    {
        public int MapId;
        public bool Completed;
        public int Stars;
        public int Time;
        public int CompletionPercent;
    }

    [Serializable]
    public class MapProgressDto
    {
        // camelCase: pontosan egyezik a backend JSON kulcsaival
        public int id;
        public int accountId;
        public int mapId;
        public string name;
        public bool completed;
        public int stars;
        public int bestTime;
        public int completionPercent;
    }

    /// <summary>
    /// Map progress mentése: GET-tel lekéri a rekord Id-ját, majd PUT-tal frissíti.
    /// Csak akkor ír felül Stars/BestTime értéket, ha az új jobb.
    /// </summary>
    public IEnumerator SaveMapProgress(
        int accountId,
        int mapId,
        string mapName,
        bool completed,
        int stars,
        int timeSeconds,
        int completionPercent,
        Action onSuccess = null,
        Action<string> onError = null)
    {
        // ── 1. lépés: GET – megkeressük a rekordot és annak Id-ját ────────
        string getUrl = $"{baseUrl}/PlayerMapProgress/{accountId}";
        UnityWebRequest getReq = UnityWebRequest.Get(getUrl);
        if (!string.IsNullOrEmpty(Token))
            getReq.SetRequestHeader("Authorization", "Bearer " + Token);

        yield return getReq.SendWebRequest();

        if (getReq.result != UnityWebRequest.Result.Success)
        {
            string err = $"MapProgress GET hiba: {getReq.error} | {getReq.downloadHandler.text}";
            Debug.LogError("❌ " + err);
            onError?.Invoke(err);
            yield break;
        }

        MapProgressDto targetRecord = null;
        try
        {
            string raw = getReq.downloadHandler.text;
            Debug.Log($"🗺️ MapProgress GET válasz: {raw}");
            MapProgressDto[] all = JsonHelper.FromJson<MapProgressDto>(raw);

            // Keresés name alapján (a backend nem mindig küldi a mapId-t)
            var foundNames = new System.Text.StringBuilder();
            foreach (var entry in all)
            {
                foundNames.Append($"[id={entry.id} name={entry.name}] ");
                // Egyezés: name alapján (pl. "Tél", "Nyár", "Ősz")
                if (string.Equals(entry.name, mapName, System.StringComparison.OrdinalIgnoreCase))
                    targetRecord = entry;
            }
            Debug.Log($"🗺️ Talált rekordok: {foundNames} | Keresett: name={mapName}");
        }
        catch (Exception e)
        {
            string err = $"MapProgress GET parse hiba: {e.Message}";
            Debug.LogError("❌ " + err);
            onError?.Invoke(err);
            yield break;
        }

        if (targetRecord == null)
        {
            string err = $"Nem található MapProgress rekord (AccountId={accountId}, Name={mapName}). " +
                         $"Ellenőrizd hogy a GameResultReporter Map Name mezője pontosan egyezik az adatbázisban lévővel (Nyár, Tél, Ősz)!";
            Debug.LogError("❌ " + err);
            onError?.Invoke(err);
            yield break;
        }

        Debug.Log($"✅ MapProgress rekord megtalálva: Id={targetRecord.id}, Stars={targetRecord.stars}, BestTime={targetRecord.bestTime}");

        // ── 2. lépés: csak akkor írjuk felül, ha jobb eredmény született ──
        // Stars: mindig a legmagasabbat tartjuk meg
        int newStars = Mathf.Max(targetRecord.stars, stars);

        // BestTime: győzelemnél a legkisebb időt tartjuk (0 = még nem volt győzelem)
        int newBestTime = targetRecord.bestTime;
        if (completed)
        {
            newBestTime = (targetRecord.bestTime == 0)
                ? timeSeconds
                : Mathf.Min(targetRecord.bestTime, timeSeconds);
        }

        // CompletionPercent: mindig a legmagasabbat tartjuk meg
        int newPercent = Mathf.Max(targetRecord.completionPercent, completionPercent);

        // Completed: ha egyszer már teljesítette, marad true
        bool newCompleted = targetRecord.completed || completed;

        // ── 3. lépés: POST /api/PlayerMapProgress/{accountId}/update ────
        // A Swagger szerint ez a helyes endpoint (PUT nem létezik)
        // Body: MapId, Completed, Stars, Time, CompletionPercent
        MapProgressUpdateRequest updateDto = new MapProgressUpdateRequest
        {
            MapId             = mapId,
            Completed         = newCompleted,
            Stars             = newStars,
            Time              = newBestTime,
            CompletionPercent = newPercent
        };

        string updateUrl  = $"{baseUrl}/PlayerMapProgress/{accountId}/update";
        string updateJson = JsonUtility.ToJson(updateDto);
        Debug.Log($"🗺️ MapProgress update POST → {updateUrl} | {updateJson}");

        yield return StartCoroutine(PostRequest(updateUrl, updateJson, true,
            (response) => {
                Debug.Log($"✅ MapProgress mentve! Stars:{newStars} | BestTime:{newBestTime}s | Percent:{newPercent}% | Completed:{newCompleted}");
                onSuccess?.Invoke();
            },
            (error) => {
                Debug.LogError("❌ MapProgress update hiba: " + error);
                onError?.Invoke(error);
            }
        ));
    }

    private IEnumerator CreatePlayerStats(int accountId, Action<PlayerStatsDto> onSuccess)
    {
        string url = $"{baseUrl}/PlayerStats";

        PlayerStatsDto newStats = new PlayerStatsDto
        {
            AccountId      = accountId,
            EnemiesKilled  = 1,
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

        string jsonData = JsonUtility.ToJson(newStats);
        Debug.Log("📤 PlayerStats létrehozása: " + jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + Token);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ PlayerStats létrehozási hiba: " + request.error + " | " + request.downloadHandler.text);
            onSuccess?.Invoke(null);
        }
        else
        {
            try
            {
                string responseJson = request.downloadHandler.text;
                Debug.Log("📥 Backend válasz: " + responseJson);

                PlayerStatsDto createdStats = JsonUtility.FromJson<PlayerStatsDto>(responseJson);

                Debug.Log($"✅ PlayerStats sikeresen létrehozva! ID: {createdStats.Id}, AccountId: {createdStats.AccountId}");

                onSuccess?.Invoke(createdStats);
            }
            catch (Exception e)
            {
                Debug.LogError("❌ JSON feldolgozási hiba: " + e.Message);
                onSuccess?.Invoke(null);
            }
        }
    }
}