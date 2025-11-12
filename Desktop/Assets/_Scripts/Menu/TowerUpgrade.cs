using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class TowerUpgrade : MonoBehaviour
{
    [System.Serializable]

    class Level
    {
        public float range = 8f;
        public int damage = 25;
        public float fireRate = 1f;
    }

    [SerializeField] private Level[] levels = new Level[3];
    [NonSerialized] public int currentLevel = 0;

    private Tower tower;
    [SerializeField] TowerRange towerRange;
    void Awake()
    {
        tower = GetComponent<Tower>();
    }

    public void Upgrade()
    {
        if (currentLevel < levels.Length)
        {
            tower.range = levels[currentLevel].range;
            tower.damage = levels[currentLevel].damage;
            tower.fireRate = levels[currentLevel].fireRate;
            towerRange.UpdateRange();
            currentLevel++;

            UnityEngine.Debug.Log("Upgraded");
        }
    }
}