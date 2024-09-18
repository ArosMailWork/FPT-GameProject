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
    public BotPushing pushState;
    [HideInInspector]
    public BotHit hitState;
    [HideInInspector]
    public BotSet setState;
    public void Awake()
    {
        idleState = new BotIdle(this);
        pushState = new BotPushing(this);
        hitState = new BotHit(this);
        chaseState = new BotChase(this);
        setState = new BotSet(this);
        if (bot.Team == 0)
        {
            defaultDestination = GameObject.Find("CastleBlue").transform;
        }
        else
        {
            defaultDestination = GameObject.Find("CastleRed").transform;
        }

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
        //return setState;
    }
}
