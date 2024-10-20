using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDisable : BaseState
{
    protected BotSM sm;
    protected Transform TF;
    public BotDisable(string name, BotSM stateMachine) : base("Disable", stateMachine)
    {
        sm = (BotSM)this.stateMachine;
    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("asdasd");
        sm.nav.isStopped = true;
    }
    public override void Exit()
    {
        base.Exit();
        sm.nav.isStopped = false;
    }
}
