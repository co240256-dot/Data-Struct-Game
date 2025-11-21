using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;
    public Transform firePoint;
    public GameObject arrowPrefab;

    // Input axis/button name
    public string attackButtonName = "Bow";

    private float nextShotTime = 0f;

    void Update()
    {
        if (Input.GetButtonDown(attackButtonName) &&
            Time.time >= nextShotTime)
        {
            StartBowAttack();
        }
    }

    // Starts the attack animation, NO arrow here
    void StartBowAttack()
    {
        if (anim != null)
            anim.SetBool("IsAttacking", true);

        // set cooldown immediately (or do it in ShootArrow if you prefer)
        nextShotTime = Time.time + StatsManager.Instance.bowCooldown;
    }

    // 🔹 THIS is called by the Animation Event "ShootArrow"
    public void ShootArrow()
    {
        if (arrowPrefab == null || firePoint == null)
        {
            Debug.LogWarning("PlayerCombat: arrowPrefab or firePoint not set.");
            return;
        }

        GameObject arrowObj =
            Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        Arrow arrow = arrowObj.GetComponent<Arrow>();
        if (arrow != null)
        {
            Vector2 dir =
                transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            arrow.direction = dir;
        }
    }

    // 🔹 Called from an Animation Event at the END of the attack
    public void EndAttack()
    {
        if (anim != null)
            anim.SetBool("IsAttacking", false);
    }
}
