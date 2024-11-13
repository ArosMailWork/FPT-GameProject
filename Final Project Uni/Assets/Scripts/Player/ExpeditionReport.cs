using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ExpeditionReport : MonoBehaviour
{
    public int EnemyDefeated = 0;
    public int BossDefeated = 0;
    public int ChestOpened = 0;
    public int ItemCollected = 0;
    
    public static ExpeditionReport Instance;
    
    public void Start()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }

    [Button]
    public void AchievementProgress()
    {
        AchievementManager.instance.AddAchievementProgress("MonsterHunter", EnemyDefeated);
        AchievementManager.instance.AddAchievementProgress("MonsterSlayer", EnemyDefeated);
        AchievementManager.instance.AddAchievementProgress("MonsterNightmare", EnemyDefeated);
        AchievementManager.instance.AddAchievementProgress("Overlord", BossDefeated);
        //AchievementManager.instance.AddAchievementProgress("ChestOpener", ChestOpened);
    }
}
