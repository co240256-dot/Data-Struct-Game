using UnityEngine;

using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    public float speed = 10f;
    public int damage = 1;      // will be overridden
    public float lifetime = 2f;

    public float minHitDistance = 0.15f;
    [HideInInspector] public Vector2 direction = Vector2.right;

    private Vector3 spawnPosition;

    void Start()
    {
        spawnPosition = transform.position;
        Destroy(gameObject, lifetime);

        // 🔹 THIS is the important part:
        if (StatsManager.Instance != null)
        {
            damage = StatsManager.Instance.arrowDamage;
        }

        // flip sprite based on direction (your existing code)
        var scale = transform.localScale;
        if (direction.x < 0)
            scale.x = -Mathf.Abs(scale.x);
        else if (direction.x > 0)
            scale.x =  Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
    void Update()
    {
        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Don't ever damage the player
        if (other.CompareTag("Player"))
            return;

        // Ignore collisions right at spawn so you don't get instant hits
        float traveled = Vector3.Distance(transform.position, spawnPosition);
        if (traveled < minHitDistance)
            return;

        // DAMAGE ENEMY + KNOCKBACK
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);

            // Knock the enemy back if it has Enemy_Movement
            Enemy_Movement em = other.GetComponent<Enemy_Movement>();
            if (em != null)
            {
                em.Knockback(transform);   // arrow is the source
            }

            Destroy(gameObject);
            return;
        }

        // Destroy arrow on solid stuff
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
