using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotSM : StateMachine
{
    //public List<Transform> targets;
    public Rigidbody target;
    public Vector3 destination;
    public NavMeshAgent agent;
    public Transform defaultDestination;
    public BotMain bot;
    public string currState;
    public bool isAlive = true;

    [HideInInspector]
    public BotIdle idleState;
    [HideInInspector]
    public BotChase chaseState;
    [HideInInspector]
    public BotAttacking AttackingState;
    [HideInInspector]
    public BotStrafe StrafeState;
    [HideInInspector]
    public BotKnockback knockbackState;
    [HideInInspector]
    public BotDeath deathState;

    public void Awake()
    {
        idleState = new BotIdle(this);
        AttackingState = new BotAttacking(this);
        chaseState = new BotChase(this);
        StrafeState = new BotStrafe(this);
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
        isAlive = false;
        ChangeState(deathState);
    }
    public void GoKnockback(Vector3 force)
    {
        if (currState != "Knockback")
        {
            knockbackState.force = force;
            ChangeState(knockbackState);
        }
        //Debug.Log("aaaa");

    }
}
