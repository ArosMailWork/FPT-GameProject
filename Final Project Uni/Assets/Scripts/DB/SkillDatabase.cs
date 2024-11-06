using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct PlayerSkill
{
    public string Skill_ID;
    public GameObject skillPrefab;
    public bool defaultUnlocked;
}

[System.Serializable]
public class SkillUnlock
{
    public PlayerSkill Skill;
    public bool isUnlocked;
}


[CreateAssetMenu(fileName = "SkillDatabase", menuName = "ScriptableObjects/SkillDatabase", order = 1)]
public class SkillDatabase : ScriptableObject
{
    public List<PlayerSkill> allSkills; // List of all skills in the game
}