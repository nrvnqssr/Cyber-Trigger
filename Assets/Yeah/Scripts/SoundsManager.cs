using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [SerializeField] private AudioSource rewardAudioSource;
    [SerializeField] private AudioSource UIAudioSource;

    [SerializeField] private AudioClip rewardSFX;
    [SerializeField] private AudioClip clickSFX;

    private void OnEnable()
    {
        Rewards.OnRewardInstantiated += PlayRewardSound;
    }

    private void OnDisable()
    {
        Rewards.OnRewardInstantiated -= PlayRewardSound;
    }

    private void PlayRewardSound()
    {
        rewardAudioSource.PlayOneShot(rewardSFX);
    }

    public void PlayClickSound()
    {
        UIAudioSource.PlayOneShot(clickSFX);
    }
}