using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUICaller : MonoBehaviour
{
    [SerializeField] private AchievenmentListIngame _achievenmentListIngame;
    public void ToggleWindow()
    {
        if(_achievenmentListIngame == null)
            _achievenmentListIngame = AchievementManager.instance.GetComponent<AchievenmentListIngame>();
        
        _achievenmentListIngame.ToggleWindow();
    }
}
