using System.Collections;
using System.Collections.Generic;
using Mono.CSharp;
using UnityEngine;

public class BotActive : BaseState
{
    protected BotSM sm;
    protected Transform TF;
    public BotActive(string name, BotSM stateMachine) : base("Active", stateMachine)
    {
        sm = (BotSM)this.stateMachine;
    }

    public override void TriggerEnter(Collider other)
    {
        base.TriggerEnter(other);
        if (other.CompareTag("Player") && sm.target == null)
        {
            GameObject go = other.gameObject;
            Character c = go.GetComponent<Character>();
            if (c != null)
            {
                //sm.targets.Add(c.tf);
                sm.target = c.tf;
            }

        }
    }
    // public override void TriggerExit(Collider other)
    // {
    //     base.TriggerExit(other);
    //     if (other.CompareTag("Player") && sm.target != null)
    //     {
    //         GameObject go = other.gameObject;
    //         Character c = go.GetComponent<Character>();
    //         if (c != null)
    //         {
    //             //sm.targets.Remove(c.tf);
    //             sm.target = c.tf;
    //         }

    //     }
    // }
}
