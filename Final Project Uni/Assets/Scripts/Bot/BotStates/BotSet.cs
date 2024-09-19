using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSet : BaseState
{
    protected BotSM sm;
    public BotSet(BotSM stateMachine) : base("Set", stateMachine)
    {
        sm = (BotSM)this.stateMachine;
    }
    public override void Enter()
    {
        base.Enter();
        sm.currState = "Set";
        sm.nav.enabled = false;
    }
    public void SetSpawn()
    {
        sm.ChangeState(sm.idleState);
    }
}
