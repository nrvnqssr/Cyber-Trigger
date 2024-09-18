using UnityEngine;
using System;

public class Gun : MonoBehaviour
{
    #region Actions
    public static Action<int> OnDamageDealt;
    public static Action<Gun> OnShot;
    public static Action<Gun> OnReload;
    public static Action<int, int> OnAmmoChanged;
    #endregion

    public GameObject bullet;

    public float shootForce, upwardForce;

    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap, damage;
    public bool allowButtonHold;

    int _bulletsLeft, _bulletsShot;

    public bool shooting, readyToShoot, reloading;

    public Camera cam;
    public Transform attackPoint;

    public GameObject muzzleFlash;

    public AudioClip shotSFX;
    public AudioClip reloadSFX;

    private bool _allowInvoke = true;

    #region
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)));
    }
    #endregion

    private void OnEnable()
    {
        Enemy.OnHit += DealDamage;
        OnAmmoChanged?.Invoke(_bulletsLeft, magazineSize);
    }

    private void OnDisable()
    {
        Enemy.OnHit -= DealDamage;
    }

    private void Awake()
    {
        _bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && _bulletsLeft < magazineSize && !reloading && readyToShoot)
            Reload();
        if (readyToShoot && shooting && !reloading && _bulletsLeft <= 0) Reload();

        if (readyToShoot && shooting && !reloading && _bulletsLeft > 0)
        {
            _bulletsShot = 0;
            Shoot();
        }
    }

    private void DealDamage(Enemy enemy)
    {
        enemy.TakeDamage(damage);
    }

    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        targetPoint = ray.GetPoint(75);

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        currentBullet.transform.forward = directionWithSpread.normalized;

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(cam.transform.up * upwardForce, ForceMode.Impulse);

        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        _bulletsLeft--;
        _bulletsShot++;

        if (_allowInvoke)
        {
            _allowInvoke = false;
            Invoke(nameof(ResetShot), timeBetweenShooting);
        }

        if (_bulletsShot > bulletsPerTap && bulletsPerTap > 0)
            Invoke(nameof(Shoot), timeBetweenShots);

        OnAmmoChanged?.Invoke(_bulletsLeft, magazineSize);
        OnShot.Invoke(this);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        _allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
        OnReload.Invoke(this);
        OnAmmoChanged?.Invoke(_bulletsLeft, magazineSize);
    }

    private void ReloadFinished()
    {
        _bulletsLeft = magazineSize;
        reloading = false;
        OnAmmoChanged?.Invoke(_bulletsLeft, magazineSize);
    }
}