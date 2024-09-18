using Unity.VisualScripting;
using UnityEngine;

public class EnemyRobotSounds : MonoBehaviour
{
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioSource attackAudioSource;

    [SerializeField] AudioClip[] footstepSFXs;
    [SerializeField] AudioClip[] robotSoundSFXs;
    [SerializeField] AudioClip attackSFX;

    [SerializeField] private Enemy enemy;

    [SerializeField] private GameObject soundPlayer;

    private int footstepIndex = 0;
    private int robotSoundIndex = 0;

    private void OnEnable()
    {
        enemy.OnPerformAttack += PlayAttackSound;
        enemy.OnDie += PlayDieSound;
    }

    private void OnDisable()
    {
        enemy.OnPerformAttack -= PlayAttackSound;
        enemy.OnDie -= PlayDieSound;
    }

    private void PlayFootstep()
    {
        if (footstepIndex == footstepSFXs.Length)
            footstepIndex = 0;

        footstepAudioSource.PlayOneShot(footstepSFXs[footstepIndex]);

        footstepIndex++;
    }

    private void PlayRobotSound()
    {
        if (robotSoundIndex == robotSoundSFXs.Length)
            robotSoundIndex = 0;

        footstepAudioSource.PlayOneShot(robotSoundSFXs[robotSoundIndex]);

        robotSoundIndex++;
    }

    private void PlayAttackSound()
    {
        attackAudioSource.PlayOneShot(attackSFX);
    }

    private void PlayDieSound()
    {
        Instantiate(soundPlayer, transform.position, Quaternion.identity);
    }
}