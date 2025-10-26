using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TowerRange : MonoBehaviour
{
    [SerializeField] private Tower Tower;
    private List<GameObject> targets = new List<GameObject>();

    void Start()
    {
        UpdateRange();
    }

    void Update()
    {
        // Töröljük a null referenciákat (elpusztult enemy-k)
        targets.RemoveAll(t => t == null);

        if (targets.Count > 0)
        {
            Tower.target = targets[0];
        }
        else
        {
            Tower.target = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            targets.Add(collision.gameObject); // FIX: Add helyett Remove volt!
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            targets.Remove(collision.gameObject);
        }
    }

    public void UpdateRange()
    {
        transform.localScale = new Vector3(Tower.range, Tower.range, Tower.range);
    }
}