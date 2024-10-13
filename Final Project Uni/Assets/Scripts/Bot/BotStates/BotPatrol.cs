using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPatrol : BaseState
{
    protected BotSM sm;
    public BotPatrol(StateMachine stateMachine) : base("Patrol", stateMachine)
    {
        sm = (BotSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        sm.currState = "Patrol";

    }
    public override void TriggerEnter(Collider other)
    {
        base.TriggerEnter(other);
        Debug.Log("aaaaa");
        if (other.CompareTag("Player"))
        {
            GameObject go = other.gameObject;
            sm.targets.Enqueue(go);
            Debug.Log(sm.targets.Count);
        }
    }
}
