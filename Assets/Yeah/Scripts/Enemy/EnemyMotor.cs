using UnityEngine;
using UnityEngine.AI;

public class EnemyMotor : MonoBehaviour
{
    [SerializeField] protected Transform playerTransform;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Enemy enemy;

    [SerializeField] protected float rayLength;

    [SerializeField] protected float changeDestinationCooldown;
    [SerializeField] protected float lastChangedDistinationTime;

    [SerializeField] protected bool playerInAttackRange;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        lastChangedDistinationTime = Time.time + changeDestinationCooldown;

        try
        {
            agent = GetComponent<NavMeshAgent>();
        }
        catch
        {
            Debug.Log("Enemy has no NavMeshAgent");
        }

        if (agent != null)
            agent.updateRotation = false;
    }

    private void Update()
    {
        playerTransform = PlayerHealth.Instance.gameObject.transform;
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        Move();
    }

    private void OnEnable()
    {
        enemy = GetComponent<Enemy>();
        enemy.OnPlayerEnterAttackRange += OnPlayerEnterAttackRange;
        enemy.OnPlayerExitAttackRange += OnPlayerExitAttackRange;
    }

    private void OnDisable()
    {
        enemy.OnPlayerEnterAttackRange -= OnPlayerEnterAttackRange;
        enemy.OnPlayerExitAttackRange -= OnPlayerExitAttackRange;
    }

    protected virtual void Move()
    {
        if (playerInAttackRange)
            return;

        if (Time.time - lastChangedDistinationTime > changeDestinationCooldown)
        {
            agent.destination = playerTransform.position;
            lastChangedDistinationTime = Time.time;
        }
    }

    protected virtual bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, rayLength))
            return true;
        else
            return false;
    }

    protected virtual void OnPlayerEnterAttackRange()
    {
        playerInAttackRange = true;
    }

    protected virtual void OnPlayerExitAttackRange()
    {
        playerInAttackRange = false;
    }
}