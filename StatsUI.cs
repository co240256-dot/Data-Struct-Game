using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public GameObject[] statsSlots;
    // 0 = Health, 1 = Damage, 2 = Speed
    public CanvasGroup statsCanvas;
    public string toggleButtonName = "ToggleStats";

    private bool isVisible = false;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        if (statsCanvas == null)
            statsCanvas = GetComponent<CanvasGroup>();

        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Start()
    {
        ApplyVisibility();
        updateAllStats();
    }

    private void Update()
    {
        if (Input.GetButtonDown(toggleButtonName))
        {
            isVisible = !isVisible;
            ApplyVisibility();
            updateAllStats();

            // Pause / unpause when stats are open
            Time.timeScale = isVisible ? 0f : 1f;
        }
    }

    private void ApplyVisibility()
    {
        if (statsCanvas == null) return;

        statsCanvas.alpha = isVisible ? 1f : 0f;
        statsCanvas.blocksRaycasts = isVisible;
        statsCanvas.interactable = isVisible;
    }

    public void updateHealth()
    {
        if (statsSlots.Length < 1 || statsSlots[0] == null) return;

        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null) return;

        TMP_Text text = statsSlots[0].GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = "Health: " + playerHealth.currentHealth;
        }
    }

    public void updateDamage()
    {
        if (statsSlots.Length < 2 || statsSlots[1] == null) return;

        TMP_Text text = statsSlots[1].GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            // Use arrowDamage because your skills modify that
            text.text = "Damage: " + StatsManager.Instance.arrowDamage;
        }
    }

    public void updateSpeed()
    {
        if (statsSlots.Length < 3 || statsSlots[2] == null) return;

        TMP_Text text = statsSlots[2].GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            // Use playerMoveSpeed because that’s what movement uses
            text.text = "Speed: " + StatsManager.Instance.playerMoveSpeed;
        }
    }

    public void updateAllStats()
    {
        updateHealth();
        updateDamage();
        updateSpeed();
    }
}
