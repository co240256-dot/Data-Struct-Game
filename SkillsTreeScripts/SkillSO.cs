using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public class SkillSO : ScriptableObject
{
  public string SkillName;
  public int maxLevel;
  public Sprite skillIcon;
}
