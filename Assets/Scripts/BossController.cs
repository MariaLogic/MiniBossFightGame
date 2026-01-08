using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Stats")]
    public int maxHealth = 500;
    public float moveSpeed = 2f;

    [Header("Attack Patterns")]
    public GameObject projectilePrefab;
    public Transform[] firePoints;
    public float attackCooldown = 2f;

    private int currentHealth;
    private float attackTimer;
    private Transform player;
    private int currentPhase = 1;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        attackTimer = attackCooldown;
    }

    void Update()
    {
        if (player == null || isDead) return;

        UpdatePhase();
        HandleMovement();
        HandleAttacks();
    }

    void UpdatePhase()
    {
        float healthPercent = (float)currentHealth / maxHealth;

        if (healthPercent > 0.66f)
            currentPhase = 1;
        else if (healthPercent > 0.33f)
            currentPhase = 2;
        else
            currentPhase = 3;
    }

    void HandleMovement()
    {
        // Boss slowly follows player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep on same vertical level
        float distance = Vector3.Distance(transform.position, player.position);

        // Keep some distance from player
        if (distance > 8f)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        // Make boss look at player
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    void HandleAttacks()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            Debug.Log("Boss is attacking! Phase: " + currentPhase);

            switch (currentPhase)
            {
                case 1:
                    Attack_SingleShot();
                    break;
                case 2:
                    Attack_TripleShot();
                    break;
                case 3:
                    Attack_CircularPattern();
                    break;
            }

            attackTimer = attackCooldown;
        }
    }

    void Attack_SingleShot()
    {
        if (firePoints.Length > 0 && projectilePrefab != null && firePoints[0] != null)
        {
            Vector3 direction = (player.position - firePoints[0].position).normalized;
            direction.y = 0;
            GameObject proj = Instantiate(projectilePrefab, firePoints[0].position, Quaternion.LookRotation(direction));

            Rigidbody projRb = proj.GetComponent<Rigidbody>();
            if (projRb != null)
            {
                projRb.linearVelocity = direction * 10f;
            }
        }
    }

    void Attack_TripleShot()
    {
        if (firePoints.Length > 0 && projectilePrefab != null && firePoints[0] != null)
        {
            Vector3 baseDirection = (player.position - firePoints[0].position).normalized;
            baseDirection.y = 0;

            for (int i = -1; i <= 1; i++)
            {
                float angle = i * 15f;
                Vector3 direction = Quaternion.Euler(0, angle, 0) * baseDirection;
                GameObject proj = Instantiate(projectilePrefab, firePoints[0].position, Quaternion.LookRotation(direction));

                Rigidbody projRb = proj.GetComponent<Rigidbody>();
                if (projRb != null)
                {
                    projRb.linearVelocity = direction * 10f;
                }
            }
        }
    }

    void Attack_CircularPattern()
    {
        if (projectilePrefab != null)
        {
            int projectileCount = 8;
            float angleStep = 360f / projectileCount;

            for (int i = 0; i < projectileCount; i++)
            {
                float angle = i * angleStep;
                Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
                GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(direction));

                Rigidbody projRb = proj.GetComponent<Rigidbody>();
                if (projRb != null)
                {
                    projRb.linearVelocity = direction * 8f;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Boss Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Boss Defeated!");
        // Don't destroy, just disable visual and stop attacking
        GetComponent<Renderer>().enabled = false;
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
