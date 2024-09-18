using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FunnyTimeScaler : MonoBehaviour
{
    public static FunnyTimeScaler instance;

    [SerializeField] private bool timeScaleChangingEnabled = false;

    [SerializeField] public int timePassed { get; private set; }

    public static Action<int> OnTick;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField, Range(0, 1)] float _multiplier;
    [SerializeField] float _calculateOdd;

    private void OnEnable()
    {
        InvokeRepeating(nameof(Tick), 1, 1);
        PlayerHealth.OnDied += DeathHandler;
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Tick));
        PlayerHealth.OnDied -= DeathHandler;
    }

    void Update()
    {
        if (!timeScaleChangingEnabled)
            return;

        _calculateOdd = Input.mouseScrollDelta.y * _multiplier;

        if (Time.timeScale + _calculateOdd > 0)
            Time.timeScale += _calculateOdd;
    }

    private void Tick()
    {
        timePassed++;
        OnTick?.Invoke(timePassed);
    }

    public void StopTime()
    {
        Time.timeScale = 0f;
    }

    public void ResetTime()
    {
        Time.timeScale = 1f;
    }

    private void DeathHandler()
    {
        PlayerPrefs.SetInt("PlayerTime", timePassed);
        PlayerPrefs.SetInt("OverallTime", PlayerPrefs.GetInt("OverallTime") + timePassed);

        if (timePassed > PlayerPrefs.GetInt("BestTime"))
            PlayerPrefs.SetInt("BestTime", timePassed);
    }
}