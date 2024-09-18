using Nova.TMP;
using UnityEngine;

public class GameResultsUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProTextBlock LastResultScoreText;
    [SerializeField] private TextMeshProTextBlock LastResultKillsText;
    [SerializeField] private TextMeshProTextBlock LastResultWaveText;
    [SerializeField] private TextMeshProTextBlock LastResultTimeText;
    [SerializeField] private TextMeshProTextBlock BestResultScoreText;
    [SerializeField] private TextMeshProTextBlock BestResultKillsText;
    [SerializeField] private TextMeshProTextBlock BestResultWaveText;
    [SerializeField] private TextMeshProTextBlock BestResultTimeText;

    private void Start()
    {
        UpdateLastGameResults();
        UpdateBestGameResults();
    }

    private void UpdateLastGameResults()
    {
        LastResultScoreText.text = $"{PlayerPrefs.GetInt("PlayerScore")}";
        LastResultKillsText.text = $"{PlayerPrefs.GetInt("PlayerKills")}";
        LastResultWaveText.text = $"{PlayerPrefs.GetInt("PlayerWaves")}";
        LastResultTimeText.text = $"{PlayerPrefs.GetInt("PlayerTime")}";
    }

    private void UpdateBestGameResults()
    {
        BestResultScoreText.text = $"{PlayerPrefs.GetInt("BestScore")}";
        BestResultKillsText.text = $"{PlayerPrefs.GetInt("BestKills")}";
        BestResultWaveText.text = $"{PlayerPrefs.GetInt("BestWaves")}";
        BestResultTimeText.text = $"{PlayerPrefs.GetInt("BestTime")}";
    }
}