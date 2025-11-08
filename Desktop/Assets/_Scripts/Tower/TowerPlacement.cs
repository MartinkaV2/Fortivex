using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rangeSprite;
    [SerializeField] private CircleCollider2D rangeCollider;
    [SerializeField] private Color gray;
    [SerializeField] private Color red;

    [NonSerialized] public bool isPlacing = true;
    private bool isRestricted = false;
    private int overlappingTowers = 0;

    void Awake()
    {
        // Ne kapcsoljuk ki a rangeCollider-t, csak állítsuk be a sprite láthatóságát
        if (rangeSprite != null)
            rangeSprite.enabled = true;
    }

    void Update()
    {
        if (isPlacing)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition;

            // Range sprite mindig látszik placement közben
            if (rangeSprite != null)
                rangeSprite.enabled = true;
        }

        if (Input.GetMouseButtonDown(1) && !isRestricted && overlappingTowers == 0)
        {
            // Placement befejezése - csak a sprite-ot rejtjük el
            if (rangeSprite != null)
                rangeSprite.enabled = false;

            isPlacing = false;
            // Ne kapcsoljuk ki a TowerPlacement komponenst, mert akkor a trigger-ek nem mûködnek
            // GetComponent<TowerPlacement>().enabled = false;

            // Ehelyett csak állítsuk be, hogy ne legyen placing módban
            this.enabled = false;
        }

        // Szín frissítése placement közben
        if (isPlacing && rangeSprite != null)
        {
            if (isRestricted || overlappingTowers > 0)
            {
                rangeSprite.color = red;
            }
            else
            {
                rangeSprite.color = gray;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlacing) return;

        if (collision.gameObject.tag == "Restricted")
        {
            isRestricted = true;
        }

        // Ellenõrizzük, hogy más torony van-e a közelben
        if (collision.gameObject.tag == "Tower" && collision.gameObject != this.gameObject)
        {
            overlappingTowers++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isPlacing) return;

        if (collision.gameObject.tag == "Restricted")
        {
            isRestricted = false;
        }

        // Csökkentjük a számlálót, ha egy torony elhagyja a területet
        if (collision.gameObject.tag == "Tower" && collision.gameObject != this.gameObject)
        {
            overlappingTowers--;
        }
    }
}