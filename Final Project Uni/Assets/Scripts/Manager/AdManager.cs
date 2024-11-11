using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdManager : MonoBehaviour
{
    public UnityEvent unlockSkillSlot;
    void Awake()
    {
        Gley.MobileAds.API.Initialize();
    }
    public void ShowRewardedVideo()
    {
        Debug.Log("aaaa");
        Gley.MobileAds.API.ShowRewardedVideo(SkillSlotComplete);
    }
    private void SkillSlotComplete(bool completed)
    {
        if (completed)
        {
            Debug.Log("Ad Complete");
            unlockSkillSlot.Invoke();
        }
        else
        {
            //something else
            Debug.Log("Ad Not Complete");
        }

        //coinsText.text = coins.ToString();
    }
}
