using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPatrol : BotActive
{
    public float wanderRadius = 25;
    public float wanderTimer = 5;

    private float timer;
    public BotPatrol(BotSM stateMachine) : base("Patrol", stateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        sm.nav.isStopped = false;
        sm.currState = "Patrol";
<<<<<<< Updated upstream
=======
        TF = sm.bot.transform;
>>>>>>> Stashed changes
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (sm.target != null)
        {
            sm.destination = ChooseAttackLocation(sm.target.position, sm.bot.minRange, sm.bot.maxRange);
            sm.ChangeState(sm.chaseState);
        }
    }
    public void ChooseAtkPosition()
    {

    }
    public Vector3 ChooseAttackLocation2(Vector3 origin3, float minRadius, float maxRadius)
    {
        Vector2 origin = new Vector2(origin3.x, origin3.z);
        //lmao
        Vector3 targetDir = (origin3 - sm.bot.transform.position).normalized;
        float angle = Vector3.Angle(targetDir, origin3) * (2f * Mathf.PI);
        var direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));

        //end lmao
        var randomDirection = (direction * origin).normalized;

        var randomDistance = Random.Range(minRadius, maxRadius);

        var point = origin + randomDirection * randomDistance;

        return new Vector3(point.x, sm.bot.transform.position.y, point.y);
    }
    public Vector3 ChooseAttackLocation(Vector3 origin3, float minRadius, float maxRadius)
    {
        Vector2 origin = new Vector2(origin3.x, origin3.z);

        var randomDirection = (Random.insideUnitCircle * origin).normalized;

        var randomDistance = Random.Range(minRadius, maxRadius);

        var point = origin + randomDirection * randomDistance;

        return new Vector3(point.x, sm.bot.transform.position.y, point.y);
    }
}
