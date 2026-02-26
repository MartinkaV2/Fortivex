using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main menu operate.
/// </summary>
public class MainMenu : MonoBehaviour
{
	// Name of scene to start on click
    public string startSceneName;
	// Credits menu
	public GameObject creditsMenu;

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		EventManager.StartListening("ButtonPressed", ButtonPressed);
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		EventManager.StopListening("ButtonPressed", ButtonPressed);
	}

	void Awake()
	{
		Debug.Assert(creditsMenu, "Wrong initial settings");
	}

	/// <summary>
	/// Handle Escape key to quit the game from the main menu.
	/// </summary>
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			QuitGame();
		}
	}

	/// <summary>
	/// Buttons pressed handler.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
	private void ButtonPressed(GameObject obj, string param)
	{
		switch (param)
		{
		case "Quit":
			QuitGame();
			break;
		case "Start":
			SceneManager.LoadScene(startSceneName);
			break;
		case "OpenCredits":
			creditsMenu.SetActive(true);
			break;
		case "CloseCredits":
			creditsMenu.SetActive(false);
			break;
		}
	}

	/// <summary>
	/// Quits the application. Works in both Editor and build.
	/// </summary>
	private void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}