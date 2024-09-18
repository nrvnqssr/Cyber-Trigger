using UnityEngine;

public class BulletSounds : MonoBehaviour
{
    [SerializeField] private GameObject soundPlayer;

    [SerializeField] private bool playOnHit;

    private void OnCollisionEnter(Collision collision)
    {
        if (!playOnHit)
            return;

        Instantiate(soundPlayer, transform.position, Quaternion.identity);
    }
}