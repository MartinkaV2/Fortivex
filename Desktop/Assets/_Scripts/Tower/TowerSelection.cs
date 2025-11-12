using UnityEngine;

public class TowerSelection : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rangeSprite;
    private bool isSelected = false;
    private static TowerSelection currentlySelected = null;

    void Start()
    {
        // Kezdetben ne legyen látható a range
        if (rangeSprite != null)
            rangeSprite.enabled = false;
    }

    void Update()
    {
        // Bal egérgombbal kattintás detektálása
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Ha erre a toronyra kattintottunk
                SelectTower();
            }
            else if (isSelected)
            {
                // Ha máshova kattintottunk és ez a torony volt kiválasztva
                DeselectTower();
            }
        }

        // ESC gomb lenyomása - kilépés a kijelölésből
        if (Input.GetKeyDown(KeyCode.Escape) && isSelected)
        {
            DeselectTower();
        }
    }

    private void SelectTower()
    {
        // Ha van másik kiválasztott torony, azt először kijelöljük
        if (currentlySelected != null && currentlySelected != this)
        {
            currentlySelected.DeselectTower();
        }

        isSelected = true;
        currentlySelected = this;

        // Range megjelenítése
        if (rangeSprite != null)
            rangeSprite.enabled = true;

        Debug.Log("Torony kiválasztva: " + gameObject.name);
    }

    private void DeselectTower()
    {
        isSelected = false;
        
        if (currentlySelected == this)
            currentlySelected = null;

        // Range elrejtése
        if (rangeSprite != null)
            rangeSprite.enabled = false;

        Debug.Log("Torony kijelölés törölve: " + gameObject.name);
    }

    // Ha a torony megsemmisül, töröljük a referenciát
    void OnDestroy()
    {
        if (currentlySelected == this)
            currentlySelected = null;
    }
}