using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotKnockback : BotDisable
{
    protected float cooldown = 1f;
    protected float countdown;
    public Vector3 force;
    public BotKnockback(BotSM stateMachine) : base("Knockback", stateMachine)
    {
        sm = (BotSM)this.stateMachine;
        force = Vector3.zero;
    }

    public override void Enter()
    {
        base.Enter();
        sm.currState = "Knockback";
        countdown = cooldown;
        Debug.Log(force);

        applyKnockback(force);


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
            sm.ChangeState(sm.patrolState);
        }
    }
    public void applyKnockback(Vector3 force)
    {
        Vector3 f = new Vector3(-force.x, 6f, -force.z);
        sm.bot.rg.AddForce(f, ForceMode.Impulse);
    }
}
