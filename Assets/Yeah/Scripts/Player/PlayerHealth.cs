using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region Singleton

    public static PlayerHealth Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [field: SerializeField] public int maxHealth { get; set; } 
    [SerializeField] private int health;

    // Событие HealthChanged вызывается, когда здоровье игрока изменяется
    public static Action<int> HealthChanged;

    // Событие OnDied вызывается, когда игрок умирает
    public static Action OnDied;

    private void Start()
    {
        health = maxHealth;
        HealthChanged.Invoke(health);
    }

    private void OnEnable()
    {
        Enemy.OnAttackPlayer += TakeDamage;
    }

    private void OnDisable()
    {
        Enemy.OnAttackPlayer -= TakeDamage;
    }

    private void TakeDamage(int damage)
    {
        if (health - damage <= 0) 
        {
            health = 0;
            Die();
        }
        else
        {
            health -= damage;
        }

        HealthChanged.Invoke(health);
    }

    public void AddMaxHealth(int value)
    {
        maxHealth += value;
        if (maxHealth < 1)
        {
            maxHealth = 1;
        }
        health = maxHealth;
        HealthChanged.Invoke(health);
    }

    private void Die()
    {
        OnDied.Invoke();
        Debug.Log("Вы мертвы");
    }
}