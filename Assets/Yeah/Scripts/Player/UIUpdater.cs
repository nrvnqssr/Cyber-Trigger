using Nova.TMP;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    //PlayerHealth
    [SerializeField] TextMeshProTextBlock healthText;

    //PlayerStats
    [SerializeField] TextMeshProTextBlock scoreText;
    [SerializeField] TextMeshProTextBlock enemiesKilledText;

    //Rewards
    [SerializeField] TextMeshProTextBlock rewardNameText;
    [SerializeField] TextMeshProTextBlock rewardDescriptionText;
    [SerializeField] TextMeshProTextBlock rerollsText;

    //Gun
    [SerializeField] TextMeshProTextBlock ammoText;
    [SerializeField] TextMeshProTextBlock damageText;
    [SerializeField] TextMeshProTextBlock attackSpeedText;

    //Other
    [SerializeField] TextMeshProTextBlock timeText;
    [SerializeField] TextMeshProTextBlock wavesText;
    [SerializeField] TextMeshProTextBlock deathInterfaceScoreText;
    [SerializeField] TextMeshProTextBlock deathInterfaceTimeText;
    [SerializeField] TextMeshProTextBlock deathInterfaceWaveText;
    [SerializeField] TextMeshProTextBlock deathInterfaceKilledText;
    [SerializeField] TextMeshProTextBlock enemiesAliveText;
    [SerializeField] GameObject scoreBar;
    [SerializeField] Image scoreBarFilling;

    //Interface gameobjects
    [SerializeField] GameObject inGameInterface;
    [SerializeField] GameObject deathInterface;
    [SerializeField] GameObject escapeMenuInterface;
    [SerializeField] GameObject settingsInterface;
    [SerializeField] GameObject mainMenuInterface;

    private Gun currentGun = new Gun();

    private void OnEnable()
    {
        PlayerHealth.HealthChanged += UpdateHealthValue;
        PlayerHealth.OnDied += DeathInterfaceShow;
        Enemy.OnAttackPlayer += IndicateHit;
        PlayerStats.OnScoreChanged += UpdateScoreValue;
        PlayerStats.OnEnemiesKilledChanged += UpdateEnemiesKilledValue;
        Rewards.OnCurrentUpgradeChanged += UpdateRewardUI;
        Rewards.OnRerollsValueChanged += UpdateRerollsValue;
        Gun.OnAmmoChanged += UpdateAmmoValue;
        SwitchWeapon.OnWeaponChanged += UpdateWeaponStatsUI;
        Rewards.OnCheckForReward += UpdateTillNextRewardUI;
        FunnyTimeScaler.OnTick += UpdateTimeValue;
        Waves.OnWavesValueChanged += UpdateWaveValue;
        Waves.OnEnemiesAliveChanged += UpdateEnemiesAliveValue;
    }

    private void OnDisable()
    {
        PlayerHealth.HealthChanged -= UpdateHealthValue;
        PlayerHealth.OnDied -= DeathInterfaceShow;
        Enemy.OnAttackPlayer -= IndicateHit;
        PlayerStats.OnScoreChanged -= UpdateScoreValue;
        PlayerStats.OnEnemiesKilledChanged -= UpdateEnemiesKilledValue;
        Rewards.OnCurrentUpgradeChanged -= UpdateRewardUI;
        Rewards.OnRerollsValueChanged -= UpdateRerollsValue;
        Gun.OnAmmoChanged -= UpdateAmmoValue;
        SwitchWeapon.OnWeaponChanged -= UpdateWeaponStatsUI;
        Rewards.OnCheckForReward -= UpdateTillNextRewardUI;
        FunnyTimeScaler.OnTick -= UpdateTimeValue;
        Waves.OnWavesValueChanged -= UpdateWaveValue;
        Waves.OnEnemiesAliveChanged -= UpdateEnemiesAliveValue;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !deathInterface.activeInHierarchy && !rewardNameText.isActiveAndEnabled)
        {
            if (!deathInterface.activeInHierarchy)
            {
                OpenEscapeMenu();
            }
            if (deathInterface.activeInHierarchy)
            {
                CloseEscapeMenu();
            }
        }
    }

    private void UpdateEnemiesAliveValue(int enemiesAlive)
    {
        enemiesAliveText.text = $"{enemiesAlive}";
    }

    private void UpdateWaveValue(int waveIndex)
    {
        wavesText.text = $"{waveIndex}";
    }

    private void UpdateTimeValue(int timePassed)
    {
        if (timeText == null)
            return;
        timeText.text = $"{timePassed}";
    }

    private void UpdateTillNextRewardUI(int scoreTillNextReward, int goalIncrease)
    {
        scoreBarFilling.fillAmount = 1 - (float)scoreTillNextReward / goalIncrease;
        Debug.Log(1 - (float)goalIncrease / scoreTillNextReward);
    }

    private void UpdateWeaponStatsUI(Gun gunComponent)
    {
        currentGun = gunComponent;

        damageText.text = $"{gunComponent.damage}";
        attackSpeedText.text = $"{gunComponent.timeBetweenShooting}";
    }

    public void OpenEscapeMenu()
    {
        FunnyTimeScaler.instance.StopTime();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true; 

        inGameInterface.SetActive(false);
        scoreBar.SetActive(false);
        escapeMenuInterface.SetActive(true);
        settingsInterface.SetActive(false);
    }

    public void CloseEscapeMenu()
    {
        FunnyTimeScaler.instance.ResetTime();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inGameInterface.SetActive(true);
        scoreBar.SetActive(true);
        escapeMenuInterface.SetActive(false);
    }

    public void OpenSettings()
    {
        FunnyTimeScaler.instance.StopTime();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        settingsInterface.SetActive(true);
        escapeMenuInterface.SetActive(false);
    }

    public void CloseSettings()
    {
        FunnyTimeScaler.instance.StopTime();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        settingsInterface.SetActive(false);
        escapeMenuInterface.SetActive(true);
        inGameInterface.SetActive(false);
        scoreBar.SetActive(false);
    }

    private void UpdateAmmoValue(int ammoLeft, int magazineSize)
    {
        ammoText.text = ammoLeft.ToString() + "/" + magazineSize.ToString();
    }

    private void UpdateRewardUI(Upgrade upgrade)
    {
        rewardNameText.text = upgrade.Name;
        rewardDescriptionText.text = upgrade.Description;
    }

    private void UpdateRerollsValue(int rerolls)
    {
        rerollsText.text = rerolls.ToString();

        UpdateWeaponStatsUI(currentGun);
    }

    private void UpdateHealthValue(int health)
    {
        healthText.text = $"{health}/{PlayerHealth.Instance.maxHealth}";
    }

    private void IndicateHit(int damage)
    {

    }

    private void DeathInterfaceShow()
    {
        FunnyTimeScaler.instance.StopTime();

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        deathInterfaceScoreText.text = scoreText.text;
        deathInterfaceTimeText.text = timeText.text;
        deathInterfaceWaveText.text = wavesText.text;
        deathInterfaceKilledText.text = enemiesKilledText.text;

        inGameInterface.SetActive(false);
        scoreBar.SetActive(false);
        deathInterface.SetActive(true);
    }

    private void UpdateScoreValue(int score)
    {
        scoreText.text = score.ToString();
    }

    private void UpdateEnemiesKilledValue(int enemiesKilled)
    {
        enemiesKilledText.text = enemiesKilled.ToString();
    }
}