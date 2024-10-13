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
    [HideInInspector]
    public BotPatrol patrolState;

    public void Awake()
    {
        idleState = new BotIdle(this);
        hitState = new BotHit(this);
        chaseState = new BotChase(this);
        patrolState = new BotPatrol(this);
        targets = new Queue<GameObject>();

    }
    public void GoIdle()
    {
        ChangeState(idleState);
    }
    protected override BaseState GetInitialState()
    {
        return idleState;
        //return setState;
    }
}
