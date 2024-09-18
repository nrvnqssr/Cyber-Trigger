using UnityEngine;

public class GunAnimationManager : MonoBehaviour
{
    public Animator animator;
    public Gun gun;
    public bool isShooting;
    public bool isReloading;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gun = GetComponent<Gun>();
    }

    private void Update()
    {
        isShooting = !gun.readyToShoot;
        isReloading = gun.reloading;
        animator.SetBool("isShooting", isShooting);
        animator.SetBool("isReloading", isReloading);
    }
}