using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy")
            return;
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        enemy.Die();
    }
}