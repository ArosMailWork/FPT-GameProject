using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotSM : StateMachine
{
    //public List<Transform> targets;
    public Transform target;
    public Vector3 destination;
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
    [HideInInspector]
    public BotKnockback knockbackState;
    [HideInInspector]
    public BotDeath deathState;

    public void Awake()
    {
        idleState = new BotIdle(this);
        hitState = new BotHit(this);
        chaseState = new BotChase(this);
        patrolState = new BotPatrol(this);
        knockbackState = new BotKnockback(this);
        deathState = new BotDeath(this);
        //targets = new List<Transform>();
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
    public void GoDeath()
    {
        ChangeState(deathState);
    }
}
