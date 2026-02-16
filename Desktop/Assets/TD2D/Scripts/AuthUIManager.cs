using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using System.Collections;

public class AuthUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    public Button loginButton;
    public Button registerButton;
    public TextMeshProUGUI statusText;

    private void Start()
    {
        if (registerButton != null) registerButton.onClick.AddListener(OnRegisterClick);
        if (loginButton != null) loginButton.onClick.AddListener(OnLoginClick);
    }

    public void OnRegisterClick()
    {
        statusText.text = "Regisztráció...";

        StartCoroutine(APIManager.Instance.Register(
            usernameInput.text,
            emailInput.text,
            passwordInput.text,
            (response) => {
                statusText.text = "Sikeres regisztráció!";
                Debug.Log("Server: " + response);
            },
            (error) => {
                statusText.text = "Hiba: " + error;
                Debug.LogError(error);
            }
        ));
    }

    public void OnLoginClick()
    {
        statusText.text = "Bejelentkezés...";

        StartCoroutine(APIManager.Instance.Login(
            usernameInput.text,
            passwordInput.text,
            (response) => {
                // ✅ LOGIN SIKERES
                string token = response.Trim().Trim('"');
                
                // 1️⃣ TOKEN MENTÉSE
                APIManager.Instance.SetToken(token);
                Debug.Log("✅ Token elmentve");

                // 2️⃣ USERNAME KINYERÉSE JWT-BŐL
                string username = ExtractUsernameFromJWT(token);
                
                if (string.IsNullOrEmpty(username))
                {
                    statusText.text = "Hiba: username kinyerése sikertelen!";
                    Debug.LogError("❌ Username nem található a JWT-ben");
                    return;
                }
                
                Debug.Log("✅ Username JWT-ből: " + username);

                // 3️⃣ ACCOUNTID LEKÉRDEZÉSE A BACKEND-TŐL
                StartCoroutine(FetchAccountIdByUsername(username));
            },
            
            (error) => {
                statusText.text = "Belépési hiba: " + error;
                Debug.LogError(error);
            }
        ));
    }

    /// <summary>
    /// Username kinyerése a JWT payload-ból
    /// </summary>
    private string ExtractUsernameFromJWT(string token)
    {
        try
        {
            // JWT szerkezet: header.payload.signature
            string[] parts = token.Split('.');
            if (parts.Length != 3)
            {
                Debug.LogError("❌ Érvénytelen JWT token formátum");
                return null;
            }

            // Payload dekódolás (Base64Url)
            string payload = parts[1];
            
            // Base64Url → Base64 konverzió
            payload = payload.Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            // Base64 dekódolás
            byte[] data = Convert.FromBase64String(payload);
            string json = Encoding.UTF8.GetString(data);
            
            Debug.Log("🔍 JWT Payload: " + json);

            // Username keresése (különböző claim nevek)
            if (json.Contains("claims/name"))
            {
                return ExtractStringValue(json, "claims/name");
            }
            else if (json.Contains("\"name\""))
            {
                return ExtractStringValue(json, "name");
            }
            else if (json.Contains("\"unique_name\""))
            {
                return ExtractStringValue(json, "unique_name");
            }
            
            Debug.LogError("❌ Username nem található a JWT-ben!");
            return null;
        }
        catch (Exception e)
        {
            Debug.LogError("❌ JWT dekódolási hiba: " + e.Message);
            return null;
        }
    }

    /// <summary>
    /// JSON string-ből kinyeri egy mező string értékét
    /// </summary>
    private string ExtractStringValue(string json, string fieldName)
    {
        try
        {
            int searchIndex = json.IndexOf(fieldName);
            if (searchIndex == -1) return null;

            int colonIndex = json.IndexOf(':', searchIndex);
            if (colonIndex == -1) return null;

            int startQuote = json.IndexOf('"', colonIndex);
            if (startQuote == -1) return null;
            
            int endQuote = json.IndexOf('"', startQuote + 1);
            if (endQuote == -1) return null;
            
            string value = json.Substring(startQuote + 1, endQuote - startQuote - 1);
            
            Debug.Log($"✅ {fieldName} megtalálva: {value}");
            return value;
        }
        catch (Exception e)
        {
            Debug.LogError($"❌ Hiba a {fieldName} kinyerése közben: " + e.Message);
            return null;
        }
    }

    /// <summary>
    /// AccountId lekérdezése a backend-től username alapján
    /// </summary>
    private IEnumerator FetchAccountIdByUsername(string username)
    {
        statusText.text = "AccountId betöltése...";
        
        yield return StartCoroutine(APIManager.Instance.GetAccountIdByUsername(
            username,
            (accountId) => {
                // ✅ SIKERES ACCOUNTID LEKÉRDEZÉS
                PlayerPrefs.SetInt("accountId", accountId);
                PlayerPrefs.Save();
                Debug.Log("✅ AccountId elmentve: " + accountId);
                
                // --- ITT AZ ÚJ RÉSZ ---
                // Statisztika kezelő inicializálása és mérés indítása
                if (PlayerStatsManager.Instance != null)
                {
                    Debug.Log("⏳ Statisztikák betöltése és mérés indítása...");
                    PlayerStatsManager.Instance.InitAndLoadStats(accountId);
                }
                else
                {
                    Debug.LogWarning("⚠️ PlayerStatsManager nincs a Scene-ben, a mérés nem indult el!");
                }
                // ---------------------

                statusText.text = "Sikeres bejelentkezés!";
                Invoke("LoadMainMenu", 1f);
            },
            (error) => {
                // ❌ HIBA
                statusText.text = "AccountId lekérdezési hiba!";
                Debug.LogError("❌ AccountId fetch error: " + error);
            }
        ));
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}