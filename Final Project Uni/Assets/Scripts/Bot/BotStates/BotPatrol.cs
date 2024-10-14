using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPatrol : BotActive
{
    public BotPatrol(BotSM stateMachine) : base("Patrol", stateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        sm.currState = "Patrol";

    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (sm.targets.Count > 0)
        {
            sm.ChangeState(sm.chaseState);
        }
    }
}
