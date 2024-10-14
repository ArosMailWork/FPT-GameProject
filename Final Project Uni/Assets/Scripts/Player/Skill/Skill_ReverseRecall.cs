using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ReverseRecall : ISkill
{
    public float ReverseRecallMultiplier = 1;

    private float defautDrag, defaultMass;
    
    private void Start()
    {
        _pc.ReverseRecallMultiplier = ReverseRecallMultiplier;
        defautDrag = _pc.PlayerRB.drag;
        defaultMass = _pc.PlayerRB.mass;
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
        if (toggle)
        {
            _pc.staminaSystem.canRegen = false;
            _pc.currentState = PlayerState.ReverseRecalling;
            _pc.PlayerRB.drag = 2;
            _pc.PlayerRB.mass = 1;
        }
        else
        {
            _pc.staminaSystem.canRegen = true;
            _pc.currentState = PlayerState.Idle;
            _pc.PlayerRB.drag = defautDrag;
            _pc.PlayerRB.mass = defaultMass;
        }
    }
}
