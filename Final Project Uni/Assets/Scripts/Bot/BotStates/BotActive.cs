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
    public override void Enter()
    {
        base.Enter();
        TF = sm.bot.transform;
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (sm.target != null)
        {
            Rotate();
        }

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
        if (other.CompareTag("Arrow"))
        {
            sm.ChangeState(sm.knockbackState);
            sm.bot.Health.Hurt(1);

        }
    }
    public void Rotate()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = sm.target.transform.position - TF.position;
        // The step size is equal to speed times frame time.
        float singleStep = 8.0f * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(TF.forward, new Vector3(targetDirection.x, 0, targetDirection.z), singleStep, 0.0f);
        // Calculate a rotation a step closer to the target and applies rotation to this object
        TF.rotation = Quaternion.LookRotation(newDirection);
    }
}
