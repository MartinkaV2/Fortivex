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
        // T�r�lj�k a null referenci�kat (elpusztult enemy-k)
        targets.RemoveAll(t => t == null);

        if (targets.Count > 0)
        {
            if (Tower.first)
            {
                // FIRST: Az enemy aki a legel�l van (legnagyobb checkpoint index)
                float minDistance = Mathf.Infinity;
                int maxIndex = -1;
                GameObject firstTarget = null;

                foreach (GameObject target in targets)
                {
                    Enemy enemy = target.GetComponent<Enemy>();
                    if (enemy == null) continue;

                    int enemyIndex = enemy.index;
                    float enemyDistance = enemy.distance;

                    // Keress�k a legnagyobb index-et, �s ha egyenl� akkor a legk�zelebb l�v�t a k�vetkez� checkpoint-hoz
                    if (enemyIndex > maxIndex || (enemyIndex == maxIndex && enemyDistance < minDistance))
                    {
                        maxIndex = enemyIndex;
                        minDistance = enemyDistance;
                        firstTarget = target;
                    }
                }

                Tower.target = firstTarget;
            }
            else if (Tower.last)
            {
                // LAST: Az enemy aki a legh�tul van (legkisebb checkpoint index)
                float maxDistance = -1f;
                int minIndex = int.MaxValue;
                GameObject lastTarget = null;

                foreach (GameObject target in targets)
                {
                    Enemy enemy = target.GetComponent<Enemy>();
                    if (enemy == null) continue;

                    int enemyIndex = enemy.index;
                    float enemyDistance = enemy.distance;

                    // Keress�k a legkisebb index-et, �s ha egyenl� akkor a legt�volabb l�v�t a k�vetkez� checkpoint-t�l
                    if (enemyIndex < minIndex || (enemyIndex == minIndex && enemyDistance > maxDistance))
                    {
                        minIndex = enemyIndex;
                        maxDistance = enemyDistance;
                        lastTarget = target;
                    }
                }

                Tower.target = lastTarget;
            }
            else if (Tower.strong)
            {
                // STRONG: Az enemy akinek a legt�bb HP-ja van
                int maxHealth = -1;
                GameObject strongTarget = null;

                foreach (GameObject target in targets)
                {
                    Enemy enemy = target.GetComponent<Enemy>();
                    if (enemy == null) continue;

                    if (enemy.health > maxHealth)
                    {
                        maxHealth = enemy.health;
                        strongTarget = target;
                    }
                }

                Tower.target = strongTarget;
            }
            else
            {
                // DEFAULT: Az els� enemy a list�ban
                Tower.target = targets[0];
            }
        }
        else
        {
            Tower.target = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            targets.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            targets.Remove(collision.gameObject);
        }
    }

    public void UpdateRange()
    {
        transform.localScale = new Vector3(Tower.range, Tower.range, Tower.range);
    }
}