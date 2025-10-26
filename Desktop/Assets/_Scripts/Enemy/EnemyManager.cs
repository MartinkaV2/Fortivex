using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager main;

    [Header("Spawn Settings")]
    public Transform spawnpoint;
    public Transform[] checkpoints;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject zombie;
    [SerializeField] private GameObject fastZombie;
    [SerializeField] private GameObject tankZombie;

    [Header("Wave Settings")]
    [SerializeField] private int wave = 1;
    [SerializeField] private int enemyCount = 6;
    [SerializeField] private float enemyCountRate = 0.2f;
    [SerializeField] private float spawnDelayMax = 1f;
    [SerializeField] private float spawnDelayMin = 0.75f;

    [Header("Enemy Distribution Rates")]
    [SerializeField] private float zombieRate = 0.5f;
    [SerializeField] private float fastZombieRate = 0.4f;
    [SerializeField] private float tankZombieRate = 0.1f;

    private bool wavedone = false;
    private List<GameObject> waveset = new List<GameObject>();
    private int enemyLeft;
    private int zombieCount;
    private int fastZombieCount;
    private int tankZombieCount;

    void Awake()
    {
        main = this;
    }

    void Start()
    {
        SetWave();
    }

    void Update()
    {
        // Ellen�rizz�k, hogy a hull�m befejez�d�tt-e
        if (wavedone)
        {
            // Keress�k meg az �sszes Enemy komponenst a jelenetben
            Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

            if (enemies.Length == 0)
            {
                // Minden ellens�g meghalt vagy el�rte a v�gpontot
                wavedone = false;
                wave++;

                Debug.Log($"Wave {wave - 1} completed! Starting wave {wave}");

                // Ind�tsuk el a k�vetkez� hull�mot kis k�sleltet�ssel
                StartCoroutine(NextWaveDelay());
            }
        }
    }

    IEnumerator NextWaveDelay()
    {
        yield return new WaitForSeconds(2f);
        SetWave();
    }

    private void SetWave()
    {
        // Sz�m�tsuk ki az ellens�gek sz�m�t
        enemyCount = 6 + Mathf.RoundToInt((wave - 1) * 6 * enemyCountRate);

        // Norm�l eloszl�s (nincs tank)
        zombieCount = Mathf.RoundToInt(enemyCount * (zombieRate + tankZombieRate));
        fastZombieCount = Mathf.RoundToInt(enemyCount * fastZombieRate);
        tankZombieCount = 0;

        // Csak minden 5. hull�mban (5, 10, 15, 20...) j�nnek tank zombie-k
        if (wave % 5 == 0)
        {
            zombieCount = Mathf.RoundToInt(enemyCount * zombieRate);
            fastZombieCount = Mathf.RoundToInt(enemyCount * fastZombieRate);
            tankZombieCount = Mathf.RoundToInt(enemyCount * tankZombieRate);
        }

        enemyLeft = zombieCount + fastZombieCount + tankZombieCount;

        // K�sz�ts�k el� a wave list�t
        waveset = new List<GameObject>();

        for (int i = 0; i < zombieCount; i++)
        {
            waveset.Add(zombie);
        }

        for (int i = 0; i < fastZombieCount; i++)
        {
            waveset.Add(fastZombie);
        }

        for (int i = 0; i < tankZombieCount; i++)
        {
            waveset.Add(tankZombie);
        }

        // Keverj�k meg v�letlenszer�en
        waveset = Shuffle(waveset);

        Debug.Log($"Wave {wave} started! Enemies: {enemyLeft} (Minion: {zombieCount}, Blue Minion: {fastZombieCount}, Green Minion: {tankZombieCount})");

        StartCoroutine(Spawn());
    }

    public List<GameObject> Shuffle(List<GameObject> waveSet)
    {
        List<GameObject> temp = new List<GameObject>();
        List<GameObject> result = new List<GameObject>();

        temp.AddRange(waveSet);

        for (int i = 0; i < waveSet.Count; i++)
        {
            int index = Random.Range(0, temp.Count);
            result.Add(temp[index]);
            temp.RemoveAt(index);
        }

        return result;
    }

    IEnumerator Spawn()
    {
        for (int i = 0; i < waveset.Count; i++)
        {
            if (waveset[i] != null && spawnpoint != null)
            {
                Instantiate(waveset[i], spawnpoint.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax));
        }

        wavedone = true;
    }

    // Publikus getterek
    public int GetCurrentWave()
    {
        return wave;
    }

    public int GetEnemiesLeft()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        return enemies.Length;
    }
}