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

    /// <summary>
    /// Lekérdezi az accountId-t username alapján
    /// </summary>
    public IEnumerator GetAccountIdByUsername(
        string username,
        Action<int> onSuccess,
        Action<string> onError)
    {
        string url = baseUrl + "/Accounts";

        UnityWebRequest request = UnityWebRequest.Get(url);
        
        // ✅ JWT TOKEN HOZZÁADÁSA
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

        // JSON feldolgozás
        try
        {
            string json = request.downloadHandler.text;
            Debug.Log("🔍 Accounts API válasz: " + json);

            // JSON array feldolgozás
            AccountDto[] accounts = JsonHelper.FromJson<AccountDto>(json);

            // Username alapján keresés
            foreach (var account in accounts)
            {
                // Case-insensitive összehasonlítás
                if (account.username.Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Log($"✅ AccountId megtalálva: {account.id} (username: {account.username})");
                    onSuccess?.Invoke(account.id);
                    yield break;
                }
            }

            // Ha nem találtuk meg
            onError?.Invoke($"Nem található account ezzel a username-mel: {username}");
        }
        catch (Exception e)
        {
            onError?.Invoke($"JSON feldolgozási hiba: {e.Message}");
        }
    }

    // =========================================================
    // ================= PLAYER STATS (KILL REGISTER) ==========
    // =========================================================

    [Serializable]
    public class PlayerStatsDto
    {
        public int id;
        public int accountId;
        public int enemiesKilled;
        public int timePlayed;
    }

    // PUBLIC HÍVÁS
    public void RegisterKill(int accountId)
    {
        StartCoroutine(RegisterKillCoroutine(accountId));
    }

    // COROUTINE
    private IEnumerator RegisterKillCoroutine(int accountId)
    {
        // 1️⃣ GET ALL playerstats
        string getUrl = $"{baseUrl}/PlayerStats";
        UnityWebRequest getReq = UnityWebRequest.Get(getUrl);
        getReq.SetRequestHeader("Authorization", "Bearer " + Token);

        yield return getReq.SendWebRequest();

        if (getReq.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Get stats error: " + getReq.error + " | " + getReq.downloadHandler.text);
            yield break;
        }

        // 2️⃣ JSON → lista
        PlayerStatsDto[] allStats = JsonHelper.FromJson<PlayerStatsDto>(getReq.downloadHandler.text);

        PlayerStatsDto myStats = null;

        foreach (var s in allStats)
        {
            if (s.accountId == accountId)
            {
                myStats = s;
                break;
            }
        }

        // ✅ HA NINCS MÉG PLAYERSTATS REKORD, LÉTREHOZZUK!
        if (myStats == null)
        {
            Debug.LogWarning($"⚠️ Nincs PlayerStats rekord az accountId={accountId}-hoz. Létrehozás...");
            
            // POST kérés - új PlayerStats létrehozása
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

        // 3️⃣ módosítás
        myStats.enemiesKilled += 1;

        // 4️⃣ PUT visszaküldés
        string putUrl = $"{baseUrl}/PlayerStats/{myStats.id}";
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
        }
    }

    /// <summary>
    /// Új PlayerStats rekord létrehozása a backend-en (POST)
    /// </summary>
    private IEnumerator CreatePlayerStats(int accountId, Action<PlayerStatsDto> onSuccess)
    {
        string url = $"{baseUrl}/PlayerStats";

        // Új PlayerStats alapértelmezett értékekkel
        PlayerStatsDto newStats = new PlayerStatsDto
        {
            accountId = accountId,
            enemiesKilled = 1,  // ✅ Rögtön 1-re állítjuk (ez az első kill)
            timePlayed = 0
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
                // A backend visszaküldi a létrehozott objektumot (id-val együtt)
                string responseJson = request.downloadHandler.text;
                Debug.Log("📥 Backend válasz: " + responseJson);

                PlayerStatsDto createdStats = JsonUtility.FromJson<PlayerStatsDto>(responseJson);
                
                Debug.Log($"✅ PlayerStats sikeresen létrehozva! ID: {createdStats.id}, AccountId: {createdStats.accountId}");
                
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