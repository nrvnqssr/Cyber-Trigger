using UnityEngine;
using System;

[Serializable]
public class Enemy : MonoBehaviour, IPooledObject
{
    #region Fields

    [field: SerializeField] public int health { get; private set; }
    [field: SerializeField] public int maxHealth { get; set; }
    [Header("Attributes")]
    [SerializeField] public int damage;
    [SerializeField] public int reward;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackLength;
    [SerializeField] private Vector3 attackAdditivePosition;
    [SerializeField] private float attackCooldown;
    private float lastAttackTime;

    [Header("Misc")]
    [SerializeField] protected Transform playerTransform;
    [SerializeField] GameObject deathVFX;
    [SerializeField] GameObject attackVFX;
    [SerializeField] Rigidbody rb;

    #endregion

    public Enemy() { }

    public Enemy(int maxHealth, int damage, int reward)
    {
        this.maxHealth = maxHealth;
        this.damage = damage;
        this.reward = reward;
    }

    #region Actions

    // ������� OnHit ����������, ����� ����� �������� �� �����
    public static Action<Enemy> OnHit;

    // ������� OnDead ����������, ����� ���� �������
    public static Action<Enemy> OnDead;

    public Action OnDie;

    // ������� OnDamageDealt ����������, ����� ����� ������� ���� �����
    public static Action<int> OnDamageDealt;

    // ������� OnDamageDealt ����������, ����� ����� ������� ���� �����
    public static Action<int> OnAttackPlayer;

    // ������� OnPlayerEnterAttackRange ����������, ����� ����� �������� � ������� ����� �����
    public Action OnPlayerEnterAttackRange;

    // ������� OnPlayerExitAttackRange ����������, ����� ����� ������� �� ������� ����� �����
    public Action OnPlayerExitAttackRange;

    // ������� OnPlayerExitAttackRange ����������, ����� ���� �������� �����
    public Action OnPerformAttack;

    #endregion

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (playerTransform.position.normalized - transform.position.normalized) * attackLength + attackAdditivePosition, attackRadius);
    }

    #endregion

    private void Awake()
    {
        try
        {
            rb = GetComponent<Rigidbody>();
        }
        catch
        {
            Debug.Log("Enemy has no Rigidbody");
        }

        health = maxHealth;    
    }

    private void Update()
    {
        playerTransform = PlayerHealth.Instance.gameObject.transform;
    }

    private void OnEnable()
    {
        health = maxHealth;
        Gun.OnDamageDealt += TakeDamage;
    }

    private void OnDisable()
    {
        Gun.OnDamageDealt -= TakeDamage;
        CancelInvoke(nameof(Attack));
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollisionEnterHandler(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        OnPlayerEnterAttackRange?.Invoke();
        
        InvokeRepeating(nameof(Attack), 0.5f, attackCooldown);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;
        
        OnPlayerExitAttackRange?.Invoke();
        
        CancelInvoke(nameof(Attack));
    }

    public void OnObjectSpawn()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }
    }

    public virtual void CollisionEnterHandler(Collision collision) 
    {
        if (collision.gameObject.tag == "Bullet")
        {
            OnHit?.Invoke(this);
        }
    }

    public virtual void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        Debug.Log("Enemy took " + damageAmount + " damage. Current health: " + health);

        if (health <= 0)
        {
            OnDie?.Invoke();
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy died");

        OnDead?.Invoke(this);

        Instantiate(deathVFX, gameObject.transform.position, Quaternion.identity);

        // ��� ���������� ������ ObjectPooler ������� ������� ������ ������ �����������, � �� ������������
        gameObject.SetActive(false);
    }

    public void Attack()
    {
        Vector3 relativePos = playerTransform.position - (transform.position);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position + relativePos.normalized * attackLength + attackAdditivePosition, attackRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Player")
            {
                OnAttackPlayer.Invoke(damage);
            }
        }

        Instantiate(attackVFX, transform.position + relativePos.normalized * attackLength - relativePos.normalized + attackAdditivePosition, Quaternion.LookRotation(relativePos - attackAdditivePosition));

        OnPerformAttack?.Invoke();
    }
}