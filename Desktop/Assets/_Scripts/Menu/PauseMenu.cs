using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeButton()
    {
        ResumeGame();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1; // Fontos: állítsuk vissza az idõskálát
        SceneManager.LoadSceneAsync(0);
    }

    private void PauseGame()
    {
        isPaused = true;
        container.SetActive(true);
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        isPaused = false;
        container.SetActive(false);
        Time.timeScale = 1;
    }
}