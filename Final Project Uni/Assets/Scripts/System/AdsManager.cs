using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;
    public UnityEvent Reward;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        Gley.MobileAds.API.Initialize();
    }
    
    public void ShowRewardedVideo()
    {
        if (Gley.MobileAds.API.IsRewardedVideoAvailable())
        Gley.MobileAds.API.ShowRewardedVideo(CompleteMethod);
    }
    
    public void ShowInterstitial()
    {
        if (Gley.MobileAds.API.IsInterstitialAvailable())
        Gley.MobileAds.API.ShowInterstitial();
    }
    
    private void CompleteMethod(bool completed)
    {
        if (completed)
        {
            Reward.Invoke();
            Reward.RemoveAllListeners();
        }
    }
}
