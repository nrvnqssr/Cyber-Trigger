using System;
using System.Collections.Generic;
using UnityEngine;

public class Rewards : MonoBehaviour
{
    [SerializeField] private int scoreGoal;
    [SerializeField] private int goalIncrease = 1000;

    [SerializeField] private PlayerCam playerCam;
    [SerializeField] private GameObject playerUI;

    [SerializeField] private List<Gun> guns;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMotor playerMotor;

    [SerializeField] private GameObject rewardInterface;

    [SerializeField] private Upgrade currentUpgrade;

    private int rerolls = 1;

    // Событие OnCurrentUpgradeChanged вызывается, когда изменяется текущее улучшение
    public static Action<Upgrade> OnCurrentUpgradeChanged;

    // Событие OnRerollsValueChanged вызывается, когда изменяется количество сбросов
    public static Action<int> OnRerollsValueChanged;

    // Событие OnGoalChanged вызывается, когда цель по количеству очков
    public static Action<int, int> OnCheckForReward;

    // Событие OnRewardInstantiated вызывается, когда игрок получает награду
    public static Action OnRewardInstantiated;

    #region Upgrades

    private WeightedRandomList<Upgrade> upgradesList = new WeightedRandomList<Upgrade>();

    [SerializeField] private Upgrade[] upgrades;

    #endregion

    private void Start()
    {
        foreach (var upgrade in upgrades)
        {
            upgradesList.Add(upgrade, upgrade.Weight);
        }

        OnRerollsValueChanged.Invoke(rerolls);
    }

    private void OnEnable()
    {
        PlayerStats.OnScoreChanged += CheckForReward;
    }

    private void OnDisable()
    {
        PlayerStats.OnScoreChanged -= CheckForReward;
    }

    private void InstantiateReward()
    {
        FunnyTimeScaler.instance.StopTime();
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        rewardInterface.SetActive(true);
        playerUI.SetActive(false);
        playerCam.enabled = false;

        currentUpgrade = upgradesList.GetRandom();
        OnCurrentUpgradeChanged?.Invoke(currentUpgrade);

        OnRewardInstantiated?.Invoke();
    }

    public void Randomize()
    {
        if (rerolls == 0)
            return;

        currentUpgrade = upgradesList.GetRandom();
        OnCurrentUpgradeChanged?.Invoke(currentUpgrade);

        rerolls--;
        OnRerollsValueChanged.Invoke(rerolls);
    }

    public void GiveReward()
    {
        foreach (var gun in guns)
        {
            gun.damage += currentUpgrade.Damage;

            if (gun.damage < 1)
                gun.damage = 1;

            gun.timeBetweenShooting -= currentUpgrade.AttackSpeed;

            if (gun.timeBetweenShooting < 0.04f)
                gun.timeBetweenShooting = 0.04f;

            gun.magazineSize += currentUpgrade.Ammo;

            if (gun.magazineSize < 1)
                gun.magazineSize = 1;
        }

        playerHealth.AddMaxHealth(currentUpgrade.Health);

        playerMotor.walkSpeed += currentUpgrade.WalkSpeed;

        if (playerMotor.walkSpeed < 1)
            playerMotor.walkSpeed = 1;

        playerMotor.sprintSpeed += currentUpgrade.WalkSpeed;

        playerMotor.sprintSpeed += currentUpgrade.SprintSpeed;
        
        if (playerMotor.sprintSpeed < 2)
            playerMotor.sprintSpeed = 2;
        
        playerMotor.jumpForce += currentUpgrade.JumpForce;

        if (playerMotor.jumpForce < 0.5f)
            playerMotor.jumpForce = 0.5f;

        FunnyTimeScaler.instance.ResetTime();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rewardInterface.SetActive(false);
        playerUI.SetActive(true);
        playerCam.enabled = true;

        rerolls++;
        OnRerollsValueChanged.Invoke(rerolls);
    }

    private void CheckForReward(int score)
    {
        if (score >= scoreGoal)
        {
            InstantiateReward();
            IncreaseGoal();
        }
        OnCheckForReward.Invoke(scoreGoal - score, goalIncrease);
    }

    private void IncreaseGoal()
    {
        scoreGoal += goalIncrease;
    }
}

[Serializable]
public class Upgrade
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string Description { get; set; }
    [field: SerializeField] public float Weight { get; set; }
    [field: SerializeField] public int Damage { get; set; }
    [field: SerializeField] public float AttackSpeed { get; set; }
    [field: SerializeField] public int Ammo { get; set; }
    [field: SerializeField] public int Health { get; set; }
    [field: SerializeField] public int WalkSpeed { get; set; }
    [field: SerializeField] public int SprintSpeed { get; set; }
    [field: SerializeField] public float JumpForce { get; set; }
}