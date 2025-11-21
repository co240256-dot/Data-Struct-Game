using UnityEngine;
using UnityEngine.UI;   
using TMPro;
using System;
using System.Collections.Generic;

public class SkillSlot : MonoBehaviour
{
    public List<SkillSlot> prerequisiteSkillSlots;
    public SkillSO skillSO;
    public Button skillButton;
    public int currentLevel;
    public bool isUnlocked;
    public Image skillIcon;
    public TMP_Text skillLevelText;
    public static event Action<SkillSlot> OnAbilityPointSpent;
    public static event Action<SkillSlot> OnSkillMaxed;
    
    [Header("Stat Effects Per Level")]
    public int bonusMaxHealthPerLevel;     
    public float bonusMoveSpeedPerLevel;   
    public int bonusArrowDamagePerLevel;   


    private void OnValidate()
{
    if (skillSO != null &&
        skillLevelText != null &&
        skillIcon != null &&
        skillButton != null)
    {
        UpdateUI();
    }
}

public void Unlock() 
{
   isUnlocked = true;
   UpdateUI();
}

public bool CanUnlockSkill()
    {
        foreach (SkillSlot slot in prerequisiteSkillSlots)
        {
            if (!slot.isUnlocked || slot.currentLevel < slot.skillSO.maxLevel)
            {
                return false;
            }
        }
        return true;
    }
    public void TryUpgradeSkill()
{
    if (isUnlocked && currentLevel < skillSO.maxLevel)
    {
        currentLevel++;

        // Apply stat bonuses from StatsManager
        ApplyStatBonuses(1);

        OnAbilityPointSpent?.Invoke(this);

        if (currentLevel >= skillSO.maxLevel)
        {
            OnSkillMaxed?.Invoke(this);
        }

        UpdateUI();
    }
}
void ApplyStatBonuses(int levelsGained)
{
    var stats = StatsManager.Instance;
    if (stats == null) 
        return;

    if (bonusMaxHealthPerLevel != 0)
    {
        int amount = bonusMaxHealthPerLevel * levelsGained;

        stats.maxHealth += amount;
        stats.currentHealth += amount;

        PlayerHealth ph = FindObjectOfType<PlayerHealth>();
        if (ph != null)
        {
            int newMax = Mathf.Min(ph.maxHealth + amount, 14);
            int delta  = newMax - ph.maxHealth;

            ph.maxHealth     = newMax;
            ph.currentHealth = Mathf.Clamp(ph.currentHealth + delta, 0, ph.maxHealth);
        }
    }

    if (bonusMoveSpeedPerLevel != 0f)
    {
        stats.playerMoveSpeed += bonusMoveSpeedPerLevel * levelsGained;
    }

    if (bonusArrowDamagePerLevel != 0)
    {
        stats.arrowDamage += bonusArrowDamagePerLevel * levelsGained;
    }
    StatsUI statsUI = FindObjectOfType<StatsUI>();
    if (statsUI != null)
{
    statsUI.updateAllStats();
}

}

    private void UpdateUI()
    {
         skillIcon.sprite = skillSO.skillIcon;
         if(isUnlocked)
        {
            skillButton.interactable = true;
            skillLevelText.text = currentLevel.ToString() + "/" + skillSO.maxLevel.ToString();
            skillIcon.color = Color.white;
        }
        else
        {
            skillButton.interactable = false;
            skillLevelText.text = "Locked";
            skillIcon.color = Color.grey;
        }
    }
}
