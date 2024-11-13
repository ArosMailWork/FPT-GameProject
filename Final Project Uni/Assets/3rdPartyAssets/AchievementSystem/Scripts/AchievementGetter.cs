using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AchievementGetter : MonoBehaviour
{
    [SerializeField] private GameObject viewObj;
    
    private void Start()
    {
        viewObj.SetActive(false);
    }

    public void TogglePlayerUI(bool toggle) //convinient XD
    {
        ToggleUIElements.Instance.ToggleUI(toggle);
    }
    
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
