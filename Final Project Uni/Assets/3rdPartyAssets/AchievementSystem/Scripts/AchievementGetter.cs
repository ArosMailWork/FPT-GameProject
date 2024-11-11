using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AchievementGetter : MonoBehaviour
{
    public void UnlockEvent(AchievementInfromation AUnlocked)
    {
        Debug.Log(AUnlocked.DisplayName);
    }
    
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
