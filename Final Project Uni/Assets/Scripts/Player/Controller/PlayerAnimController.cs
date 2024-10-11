using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public Animator playerAnimator;
    [CanBeNull] public Animator bowPivotAnimator;
    [CanBeNull] public Animator bowAnimator;

    public bool isDrawed;

    private void Awake()
    {
        if(playerAnimator == null) playerAnimator = GetComponentInChildren<Animator>();
    }

    #region Player

    public void UpdateRunInput(bool moving)
    {
        playerAnimator.SetBool("Moving", moving);
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
    public void RecallAnim(bool Recalling)
    {
        playerAnimator.SetBool("Recalling", Recalling);
    }

    public async UniTaskVoid DebuffStun(float time)
    {
        playerAnimator.SetBool("Stun", true);

        await UniTask.Delay(TimeSpan.FromSeconds(time));
        
        playerAnimator.SetBool("Stun", false);
    }

    #endregion

    #region Bow

    [Button]
    public async UniTaskVoid Draw(bool bowShoot, bool changeDraw)
    {
        if(bowPivotAnimator == null || bowAnimator == null) return;
        
        if(changeDraw) isDrawed = !isDrawed;
        bowPivotAnimator.SetBool("Charging", isDrawed);
        bowAnimator.SetBool("Charging", isDrawed);
        bowPivotAnimator.SetTrigger("ShootTrigger");
        
        if(bowShoot) ShootBow();
    }

    public async UniTaskVoid ShootBow()
    {
        bowAnimator.SetTrigger("ShootTrigger");
    }

    public void Slash()
    {
        bowPivotAnimator.SetTrigger("Slash");
    }

    #endregion
}
