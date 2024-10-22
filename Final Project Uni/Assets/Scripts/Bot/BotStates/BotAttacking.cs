
using UnityEngine;

public class BotAttacking : BotAttack
{
    private float hitCooldown = 0.5f;
    private float counter;

    public BotAttacking(BotSM stateMachine) : base("Attacking", stateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        counter = hitCooldown;
        sm.currState = "Attacking";
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (counter <= 0)
        {
            Hit();
            sm.ChangeState(sm.StrafeState);
        }
        else
        {
            counter -= Time.deltaTime;
        }

    }
    public void Hit()
    {
        if (sm.target != null)
        {
            sm.bot.gun.target = sm.target;
            
            //replace with play anim
            sm.bot.gun.Fire();
            //sm.bot.gun.FireStraight();
            
            counter = hitCooldown;
        }

    }
}
