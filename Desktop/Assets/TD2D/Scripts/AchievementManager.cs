using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text;

/// <summary>
/// Achievement kezelő – singleton.
/// Figyeli a játék eseményeit és meccs végén feloldja a teljesített achievementeket.
///
/// Achievement lista (AchievementId → feltétel):
///  1 – Első Győzelem:    első meccsgyőzelem
///  2 – Tower Master:     100 torony megépítve (PlayerPrefs-ben tárolva)
///  3 – Arany Gyűjtő:    összesen 10 000 arany összegyűjtve (TotalGold)
///  4 – Túlélő:          30. hullámig elért legalább egyszer (MaxWaveReached)
///  5 – Perfekcionista:  mindhárom pálya 3 csillaggal teljesítve
/// </summary>
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    private const string BASE_URL     = "https://fortivex.runasp.net/api";
    private const string TOWERS_PREFS = "towers_built_total";

    private int towersBuiltThisSession = 0;

    private System.Collections.Generic.Dictionary<int, bool> unlockedCache
        = new System.Collections.Generic.Dictionary<int, bool>();

    // ─────────────────────────────────────────────────────────────
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        EventManager.StartListening("TowerBuilt", OnTowerBuilt);
    }

    void OnDisable()
    {
        EventManager.StopListening("TowerBuilt", OnTowerBuilt);
    }

    // ─────────────────────────────────────────────────────────────
    // TORONY ESEMÉNY
    // ─────────────────────────────────────────────────────────────
    private void OnTowerBuilt(GameObject obj, string param)
    {
        towersBuiltThisSession++;
        int total = PlayerPrefs.GetInt(TOWERS_PREFS, 0) + 1;
        PlayerPrefs.SetInt(TOWERS_PREFS, total);
        PlayerPrefs.Save();
        Debug.Log($"🗼 Tower built – session: {towersBuiltThisSession} | total: {total}");
    }

    // ─────────────────────────────────────────────────────────────
    // MECCS VÉGÉN HÍVJUK EZT (GameResultReporter-ből)
    // ─────────────────────────────────────────────────────────────
    public void CheckAndUnlockAfterGame(
        int accountId,
        bool won,
        int maxWaveThisGame,
        long totalGold)
    {
        StartCoroutine(CheckAllAchievements(accountId, won, maxWaveThisGame, totalGold));
    }

    // ─────────────────────────────────────────────────────────────
    // FŐ LOGIKA
    // ─────────────────────────────────────────────────────────────
    private IEnumerator CheckAllAchievements(
        int accountId,
        bool won,
        int maxWaveThisGame,
        long totalGold)
    {
        yield return StartCoroutine(LoadAchievements(accountId));

        // ── 1: Első Győzelem ────────────────────────────────────
        if (won && !IsUnlocked(1))
            yield return StartCoroutine(Unlock(accountId, 1));

        // ── 2: Tower Master (100 torony összesen) ───────────────
        int totalTowers = PlayerPrefs.GetInt(TOWERS_PREFS, 0);
        if (totalTowers >= 100 && !IsUnlocked(2))
            yield return StartCoroutine(Unlock(accountId, 2));

        // ── 3: Arany Gyűjtő (10 000 arany összesen) ─────────────
        if (totalGold >= 10000 && !IsUnlocked(3))
            yield return StartCoroutine(Unlock(accountId, 3));

        // ── 4: Túlélő (30. hullámon túl) ────────────────────────
        if (maxWaveThisGame >= 30 && !IsUnlocked(4))
            yield return StartCoroutine(Unlock(accountId, 4));

        // ── 5: Perfekcionista (mindhárom pálya 3 ⭐) ─────────────
        if (!IsUnlocked(5))
            yield return StartCoroutine(CheckPerfectionistAndUnlock(accountId));
    }

    // ─────────────────────────────────────────────────────────────
    // ACHIEVEMENT LISTA BETÖLTÉSE
    // ─────────────────────────────────────────────────────────────
    private IEnumerator LoadAchievements(int accountId)
    {
        unlockedCache.Clear();

        string url = $"{BASE_URL}/PlayerAchievements/{accountId}";
        UnityWebRequest req = UnityWebRequest.Get(url);

        if (APIManager.Instance != null)
            req.SetRequestHeader("Authorization", "Bearer " + APIManager.Instance.Token);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Achievement betöltési hiba: " + req.error);
            yield break;
        }

        // ⚠️ yield nem lehet try/catch-en belül – parse-t külön végezzük
        string rawJson = req.downloadHandler.text;
        AchievementDto[] list = null;

        try
        {
            list = JsonHelper.FromJson<AchievementDto>(rawJson);
        }
        catch (Exception e)
        {
            Debug.LogError("❌ Achievement JSON hiba: " + e.Message);
            yield break;
        }

        foreach (var a in list)
            unlockedCache[a.achievementId] = a.unlocked;

        Debug.Log($"✅ {list.Length} achievement betöltve.");
    }

    // ─────────────────────────────────────────────────────────────
    // PERFEKCIONISTA: map progress lekérés
    // ─────────────────────────────────────────────────────────────
    private IEnumerator CheckPerfectionistAndUnlock(int accountId)
    {
        string url = $"{BASE_URL}/PlayerMapProgress/{accountId}";
        UnityWebRequest req = UnityWebRequest.Get(url);

        if (APIManager.Instance != null)
            req.SetRequestHeader("Authorization", "Bearer " + APIManager.Instance.Token);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ MapProgress betöltési hiba: " + req.error);
            yield break;
        }

        // ⚠️ yield nem lehet try/catch-en belül – parse és feltétel-ellenőrzés előbb
        string rawJson = req.downloadHandler.text;
        bool shouldUnlock = false;

        try
        {
            MapProgressDto[] maps = JsonHelper.FromJson<MapProgressDto>(rawJson);

            bool tel = false, nyar = false, osz = false;
            foreach (var m in maps)
            {
                if (m.name == "Tél"  && m.stars >= 3) tel  = true;
                if (m.name == "Nyár" && m.stars >= 3) nyar = true;
                if (m.name == "Ősz"  && m.stars >= 3) osz  = true;
            }

            shouldUnlock = tel && nyar && osz;
        }
        catch (Exception e)
        {
            Debug.LogError("❌ MapProgress JSON hiba: " + e.Message);
            yield break;
        }

        // yield csak a try/catch-en KÍVÜL!
        if (shouldUnlock)
        {
            Debug.Log("🌟 Perfekcionista feltétel teljesítve!");
            yield return StartCoroutine(Unlock(accountId, 5));
        }
    }

    // ─────────────────────────────────────────────────────────────
    // ACHIEVEMENT FELOLDÁSA – POST /unlock
    // ─────────────────────────────────────────────────────────────
    private IEnumerator Unlock(int accountId, int achievementId)
    {
        string url  = $"{BASE_URL}/PlayerAchievements/{accountId}/unlock";
        string json = JsonUtility.ToJson(new UnlockRequest { AchievementId = achievementId });
        byte[] body = Encoding.UTF8.GetBytes(json);

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler   = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        if (APIManager.Instance != null)
            req.SetRequestHeader("Authorization", "Bearer " + APIManager.Instance.Token);

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"❌ Achievement unlock hiba (Id:{achievementId}): " + req.error
                           + " | " + req.downloadHandler.text);
        }
        else
        {
            unlockedCache[achievementId] = true;
            Debug.Log($"🏆 Achievement feloldva! Id: {achievementId}");
            EventManager.TriggerEvent("AchievementUnlocked", null, achievementId.ToString());
        }
    }

    // ─────────────────────────────────────────────────────────────
    // SEGÉDEK
    // ─────────────────────────────────────────────────────────────
    private bool IsUnlocked(int id)
    {
        return unlockedCache.TryGetValue(id, out bool v) && v;
    }

    // ─────────────────────────────────────────────────────────────
    // DTO-K
    // ─────────────────────────────────────────────────────────────
    [Serializable]
    private class AchievementDto
    {
        public int    id;
        public int    achievementId;
        public string name;
        public bool   unlocked;
    }

    [Serializable]
    private class MapProgressDto
    {
        public int    id;
        public int    accountId;
        public int    mapId;
        public string name;
        public bool   completed;
        public int    stars;
    }

    [Serializable]
    private class UnlockRequest
    {
        public int AchievementId;
    }
}