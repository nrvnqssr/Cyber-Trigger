using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject explosion;
    [SerializeField] private bool destroyOnHit;
    [SerializeField] private bool explodeOnHit;
    [SerializeField] private float destroyTime;
    [SerializeField] private float explosionDestroyTime;

    private void OnCollisionEnter(Collision collision)
    {
        if (hitVFX != null)
            Instantiate(hitVFX, gameObject.transform.position, Quaternion.identity);

        if (explodeOnHit)
        {
            explosion = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(explosion, explosionDestroyTime);
        }

        if (destroyOnHit)
            Destroy(gameObject);

        Destroy(gameObject, destroyTime);
    }
}