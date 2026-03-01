using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Add this script to any GameObject in the scene.
/// Assign the TMP_InputFields in order — Tab will cycle through them.
/// </summary>
public class TabNavigation : MonoBehaviour
{
    [Header("Input Fields (sorrendben)")]
    public TMP_InputField[] inputFields;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HandleTabNavigation();
        }
    }

    private void HandleTabNavigation()
    {
        if (inputFields == null || inputFields.Length == 0) return;

        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i] != null && currentSelected == inputFields[i].gameObject)
            {
                // Következő field (körkörösen)
                int nextIndex = (i + 1) % inputFields.Length;
                SelectInputField(inputFields[nextIndex]);
                return;
            }
        }

        // Ha semmi nincs kijelölve, az első fieldet jelöljük ki
        SelectInputField(inputFields[0]);
    }

    private void SelectInputField(TMP_InputField field)
    {
        if (field == null) return;

        EventSystem.current.SetSelectedGameObject(field.gameObject);
        field.OnPointerClick(new UnityEngine.EventSystems.PointerEventData(EventSystem.current));
        field.ActivateInputField();
        field.MoveTextEnd(false);
    }
}