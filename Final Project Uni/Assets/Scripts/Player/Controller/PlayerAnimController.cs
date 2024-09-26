using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public Animator playerAnimator;

    private void Awake()
    {
        playerAnimator = GetComponentInChildren<Animator>();
    }

    public void UpdateRunInput(bool moving)
    {
        playerAnimator.SetBool("Moving", moving); // test
    }

    public void GuardAnim(bool guard)
    {
        playerAnimator.SetBool("Guard", guard);
    }

    public void DodgeAnim()
    {
        playerAnimator.SetTrigger("Dodge");
    }
    
    public void DieAnim(bool die)
    {
        playerAnimator.SetBool("Die", die);
    }
    
    public void DamagedAnim()
    {
        playerAnimator.SetTrigger("Damaged");
    }

    public async UniTaskVoid DebuffStun(float time)
    {
        playerAnimator.SetBool("Stun", true);

        await UniTask.Delay(TimeSpan.FromSeconds(time));
        
        playerAnimator.SetBool("Stun", false);
    }
}
