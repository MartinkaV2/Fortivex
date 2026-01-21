using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // <-- EZT ADD HOZZÁ!

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
                statusText.text = "Sikeres belépés!";
                Debug.Log("Login OK: " + response);

                // 1 másodperc késleltetés, majd átirányítás
                Invoke("LoadMainMenu", 1f);

                // ÁTIRÁNYÍTÁS A MAINMENU-RE
                SceneManager.LoadScene("MainMenu"); // <-- SCENE NÉV
            },
            (error) => {
                statusText.text = "Belépési hiba: " + error;
                Debug.LogError(error);
            }
        ));
    }
}