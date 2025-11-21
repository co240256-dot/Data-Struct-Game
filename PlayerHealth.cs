using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float regenInterval = 5f;   // seconds between heals
    private float regenTimer = 0f;
 
    // Fallback values if there is no StatsManager
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Stats (optional, but recommended)")]
    public StatsManager stats;

    private void Awake()
    {
        if (stats == null)
            stats = StatsManager.Instance;

        if (stats != null)
        {
            // Pull max health from StatsManager
            maxHealth = stats.maxHealth;

            // If StatsManager has a saved currentHealth > 0, use it
            if (stats.currentHealth > 0)
                currentHealth = stats.currentHealth;
            else
                currentHealth = maxHealth;
        }
        else
        {
            // No StatsManager found → use local inspector value
            currentHealth = maxHealth;
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Push updated HP back into StatsManager (so UI / saving can read it)
        if (stats != null)
        {
            stats.currentHealth = currentHealth;
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player died, HP = " + currentHealth);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Player HP = " + currentHealth);
        }
    }
    private void Update()
{
    // Don’t regen if we’re dead
    if (currentHealth <= 0) return;

    // Count up time
    regenTimer += Time.deltaTime;

    if (regenTimer >= regenInterval)
    {
        regenTimer = 0f;

        // Only heal if not already full
        if (currentHealth < maxHealth)
        {
            ChangeHealth(+1);
        }
    }
}

}
