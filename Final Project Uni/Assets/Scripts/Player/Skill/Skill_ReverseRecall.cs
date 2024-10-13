using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ReverseRecall : ISkill
{
    public float ReverseRecallMultiplier = 1;
    
    private void Start()
    {
        _pc.ReverseRecallMultiplier = ReverseRecallMultiplier;
    }
    
    public override void Activate()
    {
        if (currentCD <= 0)
        {
            Debug.Log(Name + " Activated");
            ReverseRecall(true);
            currentCD = Cooldown;
        }
    }

    public override void Deactivate()
    {
        ReverseRecall(false);
    }

    public void ReverseRecall(bool toggle)
    {
        if (toggle) _pc.currentState = PlayerState.ReverseRecalling;
        else _pc.currentState = PlayerState.Idle;
    }
}
