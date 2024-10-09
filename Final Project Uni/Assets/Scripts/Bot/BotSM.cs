using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotSM : StateMachine
{
    public Queue<GameObject> targets;
    public GameObject target;
    public NavMeshAgent nav;
    public Transform defaultDestination;
    public BotMain bot;
    public string currState;

    [HideInInspector]
    public BotIdle idleState;
    [HideInInspector]
    public BotChase chaseState;
    [HideInInspector]
    public BotHit hitState;

    public void Awake()
    {
        idleState = new BotIdle(this);
        hitState = new BotHit(this);
        chaseState = new BotChase(this);
        targets = new Queue<GameObject>();

    }
    public void GoIdle()
    {
        //ChangeState(idleState);
    }
    public void GoSet()
    {
        //ChangeState(setState);
    }
    protected override BaseState GetInitialState()
    {
        return idleState;
    }
}
