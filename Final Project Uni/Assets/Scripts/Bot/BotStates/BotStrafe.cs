using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStrafe : BotActive
{
    public float wanderRadius = 25;
    public float wanderTimer = 5;

    private float timer;
    public BotStrafe(BotSM stateMachine) : base("Strafe", stateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        sm.nav.isStopped = false;
        sm.currState = "Strafe";
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (sm.target != null)
        {
            sm.destination = ChooseAttackLocation(sm.target.position, sm.bot.minRange, sm.bot.maxRange, sm.bot.MoveAngle);
            sm.ChangeState(sm.chaseState);
        }
    }

    public Vector3 ChooseAttackLocation(Vector3 targetPosition, float minRadius, float maxRadius, float halfAngle = 45f)
    {
        // The enemy's current position
        Vector3 enemyPosition = sm.transform.position;

        // Calculate the direction from the target to the enemy
        Vector3 directionToEnemy = (enemyPosition - targetPosition).normalized;

        // Pick a random angle within the fan (between -halfAngle and +halfAngle)
        float randomAngle = Random.Range(-halfAngle, halfAngle);

        // Rotate the direction vector by the random angle (in degrees) around the Y axis
        Vector3 randomDirection = Quaternion.Euler(0, randomAngle, 0) * directionToEnemy;

        // Pick a random distance within the min and max radius
        float randomDistance = Random.Range(minRadius, maxRadius);

        // Calculate the final point within the fan
        Vector3 point = targetPosition + randomDirection * randomDistance;

        // Return the calculated point
        return new Vector3(point.x, enemyPosition.y, point.z); // Keep the Y position of the enemy
    }



}
