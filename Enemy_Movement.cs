using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    private int facingDirection = 1;

    public EnemyState enemyState;

    [Header("Attack")]
    public float attackRange = 0.5f;
    public int damage = 1;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;

    [Header("Knockback")]
    public float knockbackForce = 4f;
    public float knockbackDuration = 0.2f;
    private bool isKnockback;
    private float knockbackEndTime;

    private Rigidbody2D rb;
    private Transform player;
    private PlayerHealth playerHealth;
    private Animator anim;

    void Start()
    {
        rb   = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        ChangeState(EnemyState.Idle);
    }

    void FixedUpdate()
    {
        // If enemy is being knocked back, ignore AI until duration ends
        if (isKnockback)
        {
            if (Time.time >= knockbackEndTime)
            {
                isKnockback = false;
                rb.velocity = Vector2.zero;
            }
            return;
        }

        if (player == null)
        {
            rb.velocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
            return;
        }

        // Face the player
        if ((player.position.x > transform.position.x && facingDirection == -1) ||
            (player.position.x < transform.position.x && facingDirection == 1))
        {
            Flip();
        }

        float dist      = Vector2.Distance(player.position, transform.position);
bool  canAttack = Time.time >= lastAttackTime + attackCooldown;

// Only attack when VERY close AND off cooldown
if (dist <= attackRange && canAttack)
{
    ChangeState(EnemyState.Attacking);
}
else if (dist <= attackRange)
{
    // Close but waiting for cooldown -> keep chasing / sticking to player
    ChangeState(EnemyState.Chasing);
}
else
{
    // Too far -> chase
    ChangeState(EnemyState.Chasing);
}


        switch (enemyState)
        {
            case EnemyState.Idle:
                rb.velocity = Vector2.zero;
                break;

            case EnemyState.Chasing:
                Vector2 dir = ((Vector2)player.position - rb.position).normalized;
                rb.velocity = dir * speed;
                break;

            case EnemyState.Attacking:
                rb.velocity = Vector2.zero;
                break;
        }
    }

    void Flip()
    {
        facingDirection *= -1;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

   private void OnTriggerEnter2D(Collider2D col)
{
    if (!col.CompareTag("Player")) return;

    player       = col.transform;
    playerHealth = col.GetComponent<PlayerHealth>();
    ChangeState(EnemyState.Chasing);
}

private void OnTriggerExit2D(Collider2D col)
{
    if (!col.CompareTag("Player")) return;

    player       = null;
    playerHealth = null;
    rb.velocity  = Vector2.zero;

    ChangeState(EnemyState.Idle);
}
public void AttackHit()
{
    if (playerHealth == null || player == null)
        return;

    float dist = Vector2.Distance(player.position, transform.position);
    if (dist > attackRange)
        return;

    if (Time.time < lastAttackTime + attackCooldown)
        return;

    lastAttackTime = Time.time;
    playerHealth.ChangeHealth(-damage);

    PlayerMovement pm = player.GetComponent<PlayerMovement>();
    if (pm != null)
        pm.Knockback(transform);
}


    public void Knockback(Transform source)
    {
        isKnockback      = true;
        knockbackEndTime = Time.time + knockbackDuration;

        Vector2 direction = ((Vector2)transform.position - (Vector2)source.position).normalized;
        rb.velocity       = direction * knockbackForce;
    }

    void ChangeState(EnemyState newState)
    {
        if (enemyState == newState)
            return;

        // turn OFF old state's bool
        switch (enemyState)
        {
            case EnemyState.Idle:
                anim.SetBool("isIdle", false);
                break;
            case EnemyState.Chasing:
                anim.SetBool("isChasing", false);
                break;
            case EnemyState.Attacking:
                anim.SetBool("isAttacking", false);
                break;
        }

        enemyState = newState;

        // turn ON new state's bool
        switch (enemyState)
        {
            case EnemyState.Idle:
                anim.SetBool("isIdle", true);
                break;
            case EnemyState.Chasing:
                anim.SetBool("isChasing", true);
                break;
            case EnemyState.Attacking:
                anim.SetBool("isAttacking", true);
                break;
        }
    }
}

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking
}
