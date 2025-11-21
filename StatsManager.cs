using UnityEngine;
using TMPro;
public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;
    public TMP_Text healthText;

    [Header("Player Movement")]
    // Used by PlayerMovement instead of its own speed field
    public float playerMoveSpeed = 2f;

    [Header("Player Combat / Knockback")]
    public float weaponRange = 1f;
    public int damage = 1;
    public float knockbackForce = 8f;
    public float knockbackTime = 0.25f;      
    public float knockbackDuration = 0.25f;  
    public float stunTime = 0f;

    [Header("Bow / Attack")]
    public float bowCooldown = 0.5f;
    
    [Header("Arrow Stats")]
    public int arrowDamage = 1;         

    [Header("Legacy Movement Stats (optional)")]
    public int speed = 2;                    

    [Header("Health Stats")]
    public int maxHealth = 3;
    public int currentHealth = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Time.timeScale = 1f;   
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateMaxHealth(int amount)
    {
        maxHealth += amount;
        healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }
    
}
