using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotKnockback : BotDisable
{
    protected float cooldown = 1f;
    protected float countdown;
    public BotKnockback(BotSM stateMachine) : base("Knockback", stateMachine)
    {
        sm = (BotSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        sm.currState = "Knockback";
        countdown = cooldown;

    }

    // Update is called once per frame
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (countdown >= 0)
        {
            countdown -= Time.deltaTime;
        }
        else
        {
            sm.ChangeState(sm.StrafeState);
        }
    }
}
