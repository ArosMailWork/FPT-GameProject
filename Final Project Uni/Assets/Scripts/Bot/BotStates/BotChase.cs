using UnityEngine;

public class BotChase : BotAttack
{
    public BotChase(BotSM stateMachine) : base("Chase", stateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        sm.nav.SetDestination(sm.target.position);
        sm.currState = "Chase";
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (Vector3.Distance(TF.position, sm.target.position) > 2f)
        {
            sm.nav.SetDestination(sm.target.position);
        }
        else
        {
            sm.nav.isStopped = true;
            sm.ChangeState(sm.hitState);
        }
    }



}
