using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private NovaSamples.UIControls.Slider volumeSlider;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    [SerializeField] Resolution[] resolutions;


    private void OnEnable()
    {
        volumeSlider.OnValueChanged.AddListener(SetVolume);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width}x{resolutions[i].height}";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        LoadSettings();
    }

    private void OnDisable()
    {
        volumeSlider.OnValueChanged.RemoveListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("qualityIndex", qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("isFullscreen", isFullscreen == true ? 1 : 0);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
    }

    public void LoadSettings()
    {
        qualityDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("qualityIndex"));
        resolutionDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("resolutionIndex"));
        fullscreenToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("isFullscreen") == 1 ? true : false);
        volumeSlider.Value = PlayerPrefs.GetFloat("Volume");
    }
}