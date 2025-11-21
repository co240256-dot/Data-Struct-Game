using UnityEngine;

public class Player_bow : MonoBehaviour
{
    [Header("References")]
    public Transform launchPoint;          
    public GameObject arrowPrefab;         
    public Animator anim;                  
    public string attackBoolName = "IsAttacking";

    [Header("Shoot Settings")]
    public float shootCooldown = 0.6f;        
    public float arrowSpawnDelay = 0.25f;      
    public float attackTotalDuration = 0.6f;   

    private float shootTimer;
    private bool isAttacking;
    private float attackStartTime;
    private bool arrowSpawned;

    void Update()
    {
        // cooldown countdown
        shootTimer -= Time.deltaTime;

        // E starts the bow attack (NO arrow yet)
        if (Input.GetKeyDown(KeyCode.E) && shootTimer <= 0f && !isAttacking)
        {
            StartAttack();
        }

        if (isAttacking)
        {
            float t = Time.time - attackStartTime;

            // spawn arrow after delay
            if (!arrowSpawned && t >= arrowSpawnDelay)
            {
                SpawnArrow();
                arrowSpawned = true;
            }

            // end attack after full duration
            if (t >= attackTotalDuration)
            {
                EndAttack();
            }
        }
    }

    void StartAttack()
    {
        isAttacking     = true;
        arrowSpawned    = false;
        attackStartTime = Time.time;
        shootTimer      = shootCooldown;

        if (anim != null)
            anim.SetBool(attackBoolName, true);
    }

    void SpawnArrow()
    {
        if (arrowPrefab == null || launchPoint == null)
        {
            Debug.LogWarning("Player_bow: arrowPrefab or launchPoint not set.");
            return;
        }

        // Decide direction from player's facing (scale.x)
        float sign = transform.localScale.x >= 0 ? 1f : -1f;
        Vector2 dir = (sign > 0) ? Vector2.right : Vector2.left;

        // Instantiate arrow
        GameObject arrowObj =
            Instantiate(arrowPrefab, launchPoint.position, Quaternion.identity);

        Arrow arrow = arrowObj.GetComponent<Arrow>();
        if (arrow != null)
        {
            arrow.direction = dir;
        }

        // Rotate arrow so its TIP points in that direction
        arrowObj.transform.right = dir;
    }

    void EndAttack()
    {
        isAttacking = false;

        if (anim != null)
            anim.SetBool(attackBoolName, false);
    }
}
