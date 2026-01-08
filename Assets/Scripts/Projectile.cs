using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public float speed = 15f;
    public float lifetime = 5f;
    public bool isPlayerProjectile = true;

    void Start()
    {
        Destroy(gameObject, lifetime);

        // Only apply velocity if none was set (for backwards compatibility)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && rb.linearVelocity.magnitude < 0.1f)
        {
            rb.linearVelocity = transform.forward * speed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isPlayerProjectile)
        {
            // Player projectile hits boss
            if (other.CompareTag("Boss"))
            {
                BossController boss = other.GetComponent<BossController>();
                if (boss != null)
                {
                    boss.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
        }
        else
        {
            // Boss projectile hits player
            if (other.CompareTag("Player"))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
        }

        // Destroy on collision with walls
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}