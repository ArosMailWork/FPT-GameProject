using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct AchievementEvent
{
    public string Key;
    public UnityEvent Event;
}

public class AchievementReceiver : MonoBehaviour
{
    [SerializeField] private GameObject viewObj;
    public List<AchievementEvent> AchievementEventList;

    private void Start()
    {
        viewObj.SetActive(false);
    }

    public void TogglePlayerUI(bool toggle) //convinient XD
    {
        ToggleUIElements.Instance.ToggleUI(toggle);
    }
    
    public void UnlockSkill(string SkillID)
    {
        PlayerController.Instance.PlayerProgressData.UnlockSkill(SkillID);
    }

    public void AwardSoul(int soulAmount)
    {
        // Load existing Soul value
        PermaStatsData permaStats = PlayerDataCRUD.LoadPermanentStats();
        permaStats.Soul += Mathf.RoundToInt(soulAmount);
        
        PlayerDataCRUD.SavePermanentStats(permaStats);
    }
    
    //Event Test
    [Button]
    public void Add(string Name, float amount)
    {
        AchievementManager.instance.AddAchievementProgress(Name, amount);
    }
    
    [Button]
    public void Unlock(string Name)
    {
        AchievementManager.instance.Unlock(Name);
    }
}
