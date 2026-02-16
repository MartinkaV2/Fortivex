using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RSwitch : MonoBehaviour
{
    [Header("UI Elements")]
    public Button registerButton;
    
    [Header("Scene Settings")]
    public string sceneToLoad = "RegisterScene";  // Alapértelmezett scene név
    
    void Start()
    {
        // Ha nincs manuálisan beállítva a gomb, próbáljuk meg automatikusan megtalálni
        if (registerButton == null)
        {
            registerButton = GetComponent<Button>();
        }
        
        // Ellenőrizzük, hogy van-e gomb
        if (registerButton != null)
        {
            // Hozzáadjuk a klikk eseményhez a scene váltást
            registerButton.onClick.AddListener(() => MoveToScene(sceneToLoad));
        }
        else
        {
            Debug.LogError("Register Button not assigned or found!");
        }
    }

    public void MoveToScene(string sceneName)
    {
        Debug.Log($"Attempting to load scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}