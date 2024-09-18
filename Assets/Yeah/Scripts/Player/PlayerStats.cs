using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private int enemiesKilled;

    public static Action<int> OnScoreChanged;
    public static Action<int> OnEnemiesKilledChanged;

    private void OnEnable()
    {
        Enemy.OnDead += OnEnemyDead;
        PlayerHealth.OnDied += DeathHandler;
    }

    private void OnDisable()
    {
        Enemy.OnDead -= OnEnemyDead;
        PlayerHealth.OnDied -= DeathHandler;
    }

    private void OnEnemyDead(Enemy enemy)
    {
        score += enemy.reward;
        enemiesKilled++;

        OnScoreChanged.Invoke(score);
        OnEnemiesKilledChanged.Invoke(enemiesKilled);
    }

    private void DeathHandler()
    {
        PlayerPrefs.SetInt("PlayerScore", score);
        PlayerPrefs.SetInt("PlayerKills", enemiesKilled);
        PlayerPrefs.SetInt("OverallScore", PlayerPrefs.GetInt("OverallScore") + score);
        PlayerPrefs.SetInt("OverallKills", PlayerPrefs.GetInt("OverallKills") + enemiesKilled);

        if (score > PlayerPrefs.GetInt("BestScore"))
            PlayerPrefs.SetInt("BestScore", score);
        if (enemiesKilled > PlayerPrefs.GetInt("BestKills"))
            PlayerPrefs.SetInt("BestKills", enemiesKilled);
    }
}