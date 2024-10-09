using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotIdle : BaseState
{
    protected BotSM sm;
    public BotIdle(BotSM stateMachine) : base("Idle", stateMachine)
    {
        sm = (BotSM)this.stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Idle");
        base.Enter();
        sm.currState = "Idle";
        //sm.nav.enabled = true;
        //sm.nav.isStopped = false;
        sm.ChangeState(sm.chaseState);

    }
}
