using UnityEngine;

public class PlayerSoundsManager : MonoBehaviour
{
    [SerializeField] private AudioSource gunAudioSource;
    [SerializeField] private AudioSource footstepAudioSource;

    [SerializeField] float footstepCooldownSprintMultiplier;
    [SerializeField] float playFootstepCooldown;
    [SerializeField] float lastPlayedFootstep;

    [SerializeField] AudioClip[] footstepSFXs;
    [SerializeField] AudioClip jumpSFX;

    [SerializeField] PlayerMotor playerMotor;

    private void OnEnable()
    {
        Gun.OnShot += PlayShotSFX;
        Gun.OnReload += PlayReloadSFX;
        playerMotor.OnJump += PlayJumpSound;
    }

    private void OnDisable()
    {
        Gun.OnShot -= PlayShotSFX;
        Gun.OnReload -= PlayReloadSFX;
        playerMotor.OnJump -= PlayJumpSound;
    }

    private void Update()
    {
        float cooldown;

        if (playerMotor.isSprinting)
            cooldown = playFootstepCooldown / footstepCooldownSprintMultiplier;
        else
            cooldown = playFootstepCooldown;

        if (playerMotor.isWalking && playerMotor.isGrounded)
        {
            if (Time.time - lastPlayedFootstep > cooldown)
            {
                PlayFootstep();
                lastPlayedFootstep = Time.time;
            }
        }
    }

    private int footstepIndex = 0;

    private void PlayFootstep()
    {
        if (footstepIndex == footstepSFXs.Length - 1)
            footstepIndex = 0;

        footstepAudioSource.PlayOneShot(footstepSFXs[footstepIndex]);

        footstepIndex++;
    }

    private void PlayJumpSound()
    {
        if (playerMotor.isGrounded)
            footstepAudioSource.PlayOneShot(jumpSFX);
    }

    private void PlayShotSFX(Gun gun)
    {
        gunAudioSource.PlayOneShot(gun.shotSFX);
    }

    private void PlayReloadSFX(Gun gun)
    {
        gunAudioSource.PlayOneShot(gun.reloadSFX);
    }
}