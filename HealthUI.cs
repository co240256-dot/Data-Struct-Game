using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    private void Update()
    {
        if (playerHealth == null) return;

        int hp    = Mathf.Clamp(playerHealth.currentHealth, 0, hearts.Length);
        int maxHp = Mathf.Clamp(playerHealth.maxHealth,     0, hearts.Length);

        for (int i = 0; i < hearts.Length; i++)
        {
            bool withinMax = i < maxHp;

            // Only show hearts up to maxHealth
            hearts[i].enabled = withinMax;

            if (!withinMax) continue;

            hearts[i].sprite = (i < hp) ? fullHeart : emptyHeart;
        }
    }
}
