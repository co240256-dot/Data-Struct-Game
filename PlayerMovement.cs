using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool isKnockback;
    private float knockbackEndTime;   

    public int facingDirection = 1;

    public Animator anim;
    public Rigidbody2D rb;



    [Header("Input")]
    public string attackButtonName = "Bow"; 

    void FixedUpdate()
    {
        if (isKnockback)
        {
            if (Time.time >= knockbackEndTime)
            {
                isKnockback = false;
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical   = Input.GetAxisRaw("Vertical");

        // Flip
        if ((horizontal > 0 && transform.localScale.x < 0) ||
            (horizontal < 0 && transform.localScale.x > 0))
        {
            Flip();
        }

        // Animator
        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical",   Mathf.Abs(vertical));

        // Movement – speed from StatsManager
        float speed = StatsManager.Instance.playerMoveSpeed;
        Vector2 movement = new Vector2(horizontal, vertical).normalized;
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(
            transform.localScale.x * -1,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    // Called by enemy when it hits the player
    public void Knockback(Transform enemy)
    {
        isKnockback = true;
        knockbackEndTime =
            Time.time + StatsManager.Instance.knockbackDuration;

        float force = StatsManager.Instance.knockbackForce;

        Vector2 dir =
            ((Vector2)transform.position - (Vector2)enemy.position).normalized;
        rb.linearVelocity = dir * force;
    }
}
