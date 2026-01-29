using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    int accountId;
    int enemiesKilled;
    int timePlayed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    // LOGIN UTÁN
    public void Init(int accId)
    {
        accountId = accId;
        enemiesKilled = 0;
        timePlayed = 0;
    }

    public void AddKill()
    {
        enemiesKilled++;
    }

    void Update()
    {
        timePlayed += Mathf.RoundToInt(Time.deltaTime);
    }

    public void SaveStats()
    {
        StartCoroutine(PutStats());
    }

    IEnumerator PutStats()
    {
        PlayerStatsDto dto = new PlayerStatsDto
        {
            enemiesKilled = enemiesKilled,
            timePlayed = timePlayed
        };

        string json = JsonUtility.ToJson(dto);
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);

        string url =
            $"http://localhost:5105/api/PlayerStats/byAccount/{accountId}";

        UnityWebRequest req = new UnityWebRequest(url, "PUT");
        req.uploadHandler = new UploadHandlerRaw(body);
        req.downloadHandler = new DownloadHandlerBuffer();

        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader(
            "Authorization",
            "Bearer " + APIManager.Instance.Token
        );

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            Debug.LogError("STATS ERROR: " + req.error + " | " + req.downloadHandler.text);
        else
            Debug.Log("STATS SAVED");
    }
}
