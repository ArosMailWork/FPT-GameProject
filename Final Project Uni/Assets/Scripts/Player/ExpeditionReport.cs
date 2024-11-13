using System.Collections;
using System.Collections.Generic;
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

    public void AchievementProgress()
    {
        AchievementManager.instance.AddAchievementProgress("Monster Hunter", EnemyDefeated);
        AchievementManager.instance.AddAchievementProgress("Monster Slayer", EnemyDefeated);
        AchievementManager.instance.AddAchievementProgress("Monster Nightmare", EnemyDefeated);
        AchievementManager.instance.AddAchievementProgress("Overlord", BossDefeated);
        AchievementManager.instance.AddAchievementProgress("Chest Opener", ChestOpened);
    }
}
