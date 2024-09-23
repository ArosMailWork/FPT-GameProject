using UnityEngine;

public class BotChase : BotAttack
{
    public BotChase(BotSM stateMachine) : base("Chase", stateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        sm.nav.SetDestination(sm.target.transform.position);
        sm.currState = "Chase";
        //Debug.Log("Chase");
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (Vector3.Distance(TF.position, sm.target.transform.position) > 2f)
        {
            sm.nav.SetDestination(sm.target.transform.position);
        }
        else
        {
            sm.nav.isStopped = true;
            sm.ChangeState(sm.hitState);
        }
    }



}
