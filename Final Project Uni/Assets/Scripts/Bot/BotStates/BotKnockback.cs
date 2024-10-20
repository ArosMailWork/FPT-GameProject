using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotKnockback : BaseState
{
    protected BotSM sm;
    protected Transform TF;
    public BotKnockback(BotSM stateMachine) : base("Knockback", stateMachine)
    {
        sm = (BotSM)this.stateMachine;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
