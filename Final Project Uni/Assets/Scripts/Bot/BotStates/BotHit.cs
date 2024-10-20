
using UnityEngine;

public class BotHit : BotAttack
{
    private float hitCooldown = 0.5f;
    private float counter;

    public BotHit(BotSM stateMachine) : base("Hit", stateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        counter = hitCooldown;
        sm.currState = "Hit";
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (counter <= 0)
        {
            Hit();
            sm.ChangeState(sm.patrolState);
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
            //GameObject.Instantiate(sm.bot.projectile, sm.gameObject.transform.position, sm.gameObject.transform.rotation);
            sm.bot.gun.FireGun();
            counter = hitCooldown;
        }

    }
}
