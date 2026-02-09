using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;

    private string baseUrl = "https://fortivex.runasp.net/api";

    // 🔐 JWT TOKEN
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

    // 🔐 TOKEN BEÁLLÍTÁSA LOGIN UTÁN
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
// COROUTINE
private IEnumerator RegisterKillCoroutine(int accountId)
{
    // 1️⃣ GET ALL playerstats
    string getUrl = $"{baseUrl}/PlayerStats";   // <-- EZ A HELYES URL
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

    if (myStats == null)
    {
        Debug.LogError("Nincs PlayerStats rekord ehhez az accountId-hoz: " + accountId);
        yield break;
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
}
