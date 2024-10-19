using UnityEngine;

public class BotAttack : BotActive
{

    public BotAttack(string name, BotSM stateMachine) : base("Attack", stateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        sm.currState = "Attack";
<<<<<<< Updated upstream
        TF = sm.bot.transform;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (sm.target != null)
        {
            Rotate();
        }
=======
>>>>>>> Stashed changes

        // if (sm.target == null)
        // {
        //     sm.ChangeState(sm.idleState);
        // }
        // else
        // {
        //     Rotate();
        // }
        // if (Vector3.Distance(TF.position, sm.target.position) > sm.bot.maxRange)
        // {
        //     sm.nav.SetDestination(sm.destination);
        // }
        // else
        // {
        //     sm.nav.isStopped = true;
        //     sm.ChangeState(sm.hitState);
        // }


        // if (Vector3.Distance(TF.position, sm.target.position) < sm.bot.minRange)
        // {
        //     sm.ChangeState(sm.hitState);
        // }
    }

<<<<<<< Updated upstream
    public void Rotate()
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = sm.target.transform.position - TF.position;
        // The step size is equal to speed times frame time.
        float singleStep = 2.0f * Time.deltaTime;
        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(TF.forward, targetDirection, singleStep, 0.0f);
        // Calculate a rotation a step closer to the target and applies rotation to this object
        TF.rotation = Quaternion.LookRotation(newDirection);
    }
=======

>>>>>>> Stashed changes

}
