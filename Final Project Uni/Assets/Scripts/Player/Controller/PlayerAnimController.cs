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

    public void UpdateRunInput(Vector2 moveVec)
    {
        //playerAnimator.SetFloat("Move_X", moveVec.x);
        //playerAnimator.SetFloat("Move_Y", moveVec.y);
        playerAnimator.SetBool("RunTest", (moveVec != Vector2.zero)); // test
    }

    public async UniTaskVoid DebuffStun(float time)
    {
        playerAnimator.SetBool("Stun", true);

        await UniTask.Delay(TimeSpan.FromSeconds(time));
        
        playerAnimator.SetBool("Stun", false);
    }
}
