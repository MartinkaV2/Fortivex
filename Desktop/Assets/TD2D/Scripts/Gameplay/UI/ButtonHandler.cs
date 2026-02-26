using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button handler.
/// </summary>
public class ButtonHandler : MonoBehaviour
{
	public AudioClip audioClip;

	// Maximum time (seconds) to wait for a sound effect before proceeding anyway
	private const float MaxSoundWait = 3f;

	/// <summary>
	/// Buttons pressed.
	/// </summary>
	/// <param name="buttonName">Button name.</param>
	public void ButtonPressed(string buttonName)
	{
		StartCoroutine(PressedCoroutine(buttonName));
	}

	/// <summary>
	/// Presseds the coroutine.
	/// </summary>
	/// <returns>The coroutine.</returns>
	/// <param name="buttonName">Button name.</param>
	private IEnumerator PressedCoroutine(string buttonName)
	{
		// Play sound effect
		if (audioClip != null && AudioManager.instance != null)
		{
			Button button = GetComponent<Button>();
			if (button != null) button.interactable = false;
			AudioManager.instance.PlaySound(audioClip);
			// Wait for sound effect to end, but no longer than MaxSoundWait
			yield return new WaitForSecondsRealtime(Mathf.Min(audioClip.length, MaxSoundWait));
			if (button != null) button.interactable = true;
		}
		// Send global event about button pressing
		EventManager.TriggerEvent("ButtonPressed", gameObject, buttonName);
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		StopAllCoroutines();
	}
}