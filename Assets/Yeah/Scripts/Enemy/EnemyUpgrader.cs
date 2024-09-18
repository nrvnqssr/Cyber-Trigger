using UnityEngine;

public class EnemyUpgrader : MonoBehaviour
{
    [SerializeField] EnemyAttributes[] enemies;

    private void Start()
    {
        foreach (var enemy in enemies)
        {
            enemy.enemyComponent.maxHealth = enemy.healthStartValue;
            enemy.enemyComponent.damage = enemy.damageStartValue;
            enemy.enemyComponent.reward = enemy.rewardStartValue;
        }
    }

    private void OnEnable()
    {
        Waves.OnNextWaveStart += UpgradeAttributes;
    }

    private void OnDisable()
    {
        Waves.OnNextWaveStart -= UpgradeAttributes;
    }

    private void UpgradeAttributes(int wave)
    {
        foreach (var enemy in enemies)
        {
            enemy.enemyComponent.maxHealth += enemy.healthIncrease;
            enemy.enemyComponent.damage += enemy.damageIncrease;
            enemy.enemyComponent.reward += enemy.rewardIncrease;
        }
    }
}

[System.Serializable]
public class EnemyAttributes 
{
    [Header("Enemy")]
    [SerializeField] public Enemy enemyComponent;

    [Header("Attributes start values")]
    [SerializeField] public int healthStartValue;
    [SerializeField] public int damageStartValue;
    [SerializeField] public int rewardStartValue;

    [Header("Attributes increases")]
    [SerializeField] public int healthIncrease;
    [SerializeField] public int damageIncrease;
    [SerializeField] public int rewardIncrease;
}