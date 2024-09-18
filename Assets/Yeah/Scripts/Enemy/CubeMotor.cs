using UnityEngine;

public class CubeMotor : EnemyMotor
{
    [SerializeField] private Rigidbody rb;
    [SerializeField, Range(0, 100)] private float jumpForce;
    [SerializeField, Range(0, 100)] private float speed;

    [SerializeField] float jumpCooldown;
    [SerializeField] float lastJumpTime;

    private void Update()
    {
        playerTransform = PlayerHealth.Instance.gameObject.transform;
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));

        if (IsGrounded())
        {
            Jump();
        }
        else
        {
            OnJumpEnded();
        }
    }

    private void Jump()
    {
        if (playerInAttackRange)
            return;

        if (Time.time - lastJumpTime < jumpCooldown)
            return;

        animator.SetBool("isGrounded", false);

        rb.AddForce(new Vector3((playerTransform.position.x - gameObject.transform.position.x) / 100 * speed, 
                                 jumpForce, 
                                (playerTransform.position.z - gameObject.transform.position.z) / 100 * speed), 
                                 ForceMode.Impulse);
    }

    private void OnJumpEnded()
    {
        lastJumpTime = Time.time;

        animator.SetBool("isGrounded", true);
    }

    protected override bool IsGrounded()
    {
        if (Physics.Raycast(transform.position - new Vector3(0.5f, 0, 0.5f), Vector3.down, rayLength) ||
            Physics.Raycast(transform.position - new Vector3(-0.5f, 0, 0.5f), Vector3.down, rayLength) ||
            Physics.Raycast(transform.position - new Vector3(0.5f, 0, -0.5f), Vector3.down, rayLength) ||
            Physics.Raycast(transform.position - new Vector3(-0.5f, 0, -0.5f), Vector3.down, rayLength))
            return true;
        else
            return false;
    }
}
