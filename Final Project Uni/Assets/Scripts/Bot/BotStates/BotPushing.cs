using UnityEngine;

public class BotPushing : BaseState
{
    protected BotSM sm;
    public BotPushing(BotSM stateMachine) : base("Push", stateMachine)
    {
        sm = (BotSM)this.stateMachine;
    }
    public override void Enter()
    {
        base.Enter();
        //implement going into Attack mode when in detection
        sm.currState = "Push";
        sm.nav.SetDestination(sm.defaultDestination.position);
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        sm.nav.SetDestination(sm.defaultDestination.position);
        if (sm.targets.Count > 0)
        {
            sm.ChangeState(sm.chaseState);
        }

    }
    public override void TriggerEnter(Collider other)
    {
        base.TriggerEnter(other);

        if (other.CompareTag("Unit"))
        {
            GameObject go = other.gameObject;
            BotMain bot = go.GetComponent<BotMain>();
            if ((int)bot.unitType <= 8 && bot.Team != sm.bot.Team)
            {
                sm.targets.Enqueue(go);
            }
        }
    }


}
