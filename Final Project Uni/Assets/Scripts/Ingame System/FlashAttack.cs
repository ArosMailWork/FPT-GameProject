using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class FlashAttack : MonoBehaviour
{
    [CanBeNull] public HurtBox hb;
    public float delay = 0;
    public float HitDuration = 0.5f;

    public UnityEvent Strike, EndStrike;

    [Button]
    public void doFlash()
    {
        doFlashAttack(HitDuration, delay);
    }

    async UniTaskVoid doFlashAttack(float hitDuration, float delay = 0)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));

        if(hb != null) hb.Activate = true;
        Strike.Invoke();
        
        await UniTask.Delay(TimeSpan.FromSeconds(hitDuration));
        
        if(hb != null) hb.Activate = false;
        EndStrike.Invoke();
    }
}
