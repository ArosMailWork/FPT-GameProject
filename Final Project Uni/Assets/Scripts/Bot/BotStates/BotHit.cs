
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
        Debug.Log("Hit!");
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
            //GameObject p = PoolManager.Spawn(sm.bot.projectile, sm.gameObject.transform.position, sm.gameObject.transform.rotation);
            //p.GetComponent<Projectile>().team = sm.bot.team;
            Debug.Log("Attack");
            counter = hitCooldown;
        }

    }
}
