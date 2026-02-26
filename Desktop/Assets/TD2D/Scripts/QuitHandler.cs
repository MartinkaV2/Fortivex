using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Standalone quit handler. Assign QuitGame() directly on the Quit button's
/// OnClick event — do NOT use ButtonHandler for quitting.
/// Also handles Escape key and ensures Alt+F4 works via wantsToQuit.
/// </summary>
public class QuitHandler : MonoBehaviour
{
    void OnEnable()
    {
        Application.wantsToQuit += OnWantsToQuit;
    }

    void OnDisable()
    {
        Application.wantsToQuit -= OnWantsToQuit;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    /// <summary>
    /// Assign this directly to the Quit button's OnClick in the Inspector.
    /// Skip ButtonHandler entirely for the quit button.
    /// </summary>
    public void QuitGame()
    {
        UnityEngine.Debug.Log("QuitGame called");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Primary: Unity's own quit
        Application.Quit();

        // Fallback: force kill the process if Application.Quit() stalls
        // (e.g. wantsToQuit handler blocking, or platform quirk)
        ForceQuit();
#endif
    }

    /// <summary>
    /// Allows Unity's quit to proceed (called when Alt+F4 or Application.Quit() fires).
    /// </summary>
    private bool OnWantsToQuit()
    {
        return true; // Always allow quitting
    }

    /// <summary>
    /// Nuclear option: kills the process directly.
    /// Only reached if Application.Quit() itself somehow stalls.
    /// </summary>
    private void ForceQuit()
    {
        Process.GetCurrentProcess().Kill();
    }
}