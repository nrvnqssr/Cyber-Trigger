using System;
using UnityEngine;

public class Waves : MonoBehaviour
{
    [Header("Waves")]
    [SerializeField] private Wave[] waves;
    [SerializeField] private int enemiesToStopSpawning = 30;

    [SerializeField] private GameObject[] spawnPoints;

    private static int waveCount;

    private Wave currentWave;
    private int currentWaveIndex;
    private static int enemiesAlive;

    // Событие OnNextWaveStart вызывается, когда начинается новая волна
    public static Action<int> OnNextWaveStart;

    // Событие OnWavesValueChanged вызывается, когда значение waveCount изменяется
    public static Action<int> OnWavesValueChanged;

    // Событие OnNextWaveStart вызывается, когда начинается новая волна
    public static Action<int> OnEnemiesAliveChanged;

    private void Start()
    {
        waveCount = 0;
        currentWaveIndex = -1;
        enemiesAlive = 0;
    }

    private void OnEnable()
    {
        PlayerHealth.OnDied += DeathHandler;
        Enemy.OnDead += DecreaseEnemiesAlive;
    }

    private void OnDisable()
    {
        PlayerHealth.OnDied -= DeathHandler;
        Enemy.OnDead -= DecreaseEnemiesAlive;
    }

    private void Update()
    {
        if (enemiesAlive < enemiesToStopSpawning)
        {
            SpawnNextWave();
        }
    }

    private void SpawnNextWave()
    {
        if ((currentWaveIndex + 1) >= waves.Length || enemiesAlive >= enemiesToStopSpawning)
            return;

        currentWaveIndex++;
        waveCount++;
        currentWave = waves[currentWaveIndex];

        foreach (var spawnPoint in spawnPoints)
        {
            enemiesAlive += currentWave.Spawn(spawnPoint.transform.position);
        }

        OnEnemiesAliveChanged.Invoke(enemiesAlive);

        OnNextWaveStart?.Invoke(currentWaveIndex + 1);

        OnWavesValueChanged.Invoke(waveCount);
    }

    private void DecreaseEnemiesAlive(Enemy enemy)
    {
        enemiesAlive--;
        OnEnemiesAliveChanged.Invoke(enemiesAlive);
    }

    private void DeathHandler()
    {
        PlayerPrefs.SetInt("PlayerWaves", waveCount);
        PlayerPrefs.SetInt("OverallWaves", PlayerPrefs.GetInt("OverallWaves") + waveCount);

        if (waveCount > PlayerPrefs.GetInt("BestWaves"))
            PlayerPrefs.SetInt("BestWaves", waveCount);
    }
}