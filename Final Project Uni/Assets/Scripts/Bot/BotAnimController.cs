using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class BotAnimController : MonoBehaviour
{
    public Animator botAnimator;
    
    private void Awake()
    {
        if(botAnimator == null) botAnimator = GetComponentInChildren<Animator>();
    }
    
    public void UpdateRunInput(bool moving)
    {
        botAnimator.SetBool("Moving", moving);
    }
    
    public void GuardAnim(bool guard)
    {
        botAnimator.SetBool("Guard", guard);
    }
    
    [Button]
    public void DamagedAnim()
    {
        botAnimator.SetTrigger("Damaged");
    }
    
    public void AttackAnim(bool Attacking)
    {
        if(Attacking) botAnimator.SetTrigger("AttackStart");
        botAnimator.SetBool("Attacking", Attacking);
    }
    
    public async UniTaskVoid DebuffStun(float time)
    {
        botAnimator.SetBool("Stun", true);

        await UniTask.Delay(TimeSpan.FromSeconds(time));
        
        botAnimator.SetBool("Stun", false);
    }
}
