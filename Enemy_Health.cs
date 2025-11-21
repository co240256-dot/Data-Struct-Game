using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int expReward = 3;
    public delegate void MonsterKilled(int exp);
    public static event MonsterKilled OnMonsterKilled;
    public int maxHealth = 3;
    public int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            OnMonsterKilled(expReward);
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
