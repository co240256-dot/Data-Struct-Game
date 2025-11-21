using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ExpManager : MonoBehaviour
{
    public int level;
    public int currentExp;
    public int expToLevel = 10;
    public float expGrowthMultiplier = 1.5f;
    public Slider expSlider;
    public TMP_Text currentLevelText;

    public static event Action <int> OnLevelUp;

    
    private void Start()
    {
        UpdateUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GainExp(2);
        }
    }
    private void OnEnable()
    {
        EnemyHealth.OnMonsterKilled += GainExp;
    }
    private void OnDisable()
    {
        EnemyHealth.OnMonsterKilled -= GainExp;
    }

    public void GainExp(int amount)
{
    currentExp += amount;

    // Level up as many times as needed
    while (currentExp >= expToLevel)
    {
        LevelUp();              
    }

    UpdateUI();            
}

        private void LevelUp()
    {
        level++;
        currentExp -= expToLevel;
        expToLevel = Mathf.RoundToInt(expToLevel * expGrowthMultiplier);
        OnLevelUp?.Invoke(1);
    }
    public void UpdateUI()
    {
        expSlider.maxValue = expToLevel;
        expSlider.value = currentExp;
        currentLevelText.text = "Level: " + level;
    }
    }

