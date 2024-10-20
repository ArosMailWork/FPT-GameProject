using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDeath : BotDisable
{
    protected float cooldown = 0.5f;
    protected float countdown;
    public BotDeath(BotSM stateMachine) : base("Knockback", stateMachine)
    {
        sm = (BotSM)this.stateMachine;
    }
    public override void Enter()
    {
        base.Enter();
        sm.currState = "Death";
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
            GameObject.Destroy(sm.bot.gameObject);
        }
    }
}
