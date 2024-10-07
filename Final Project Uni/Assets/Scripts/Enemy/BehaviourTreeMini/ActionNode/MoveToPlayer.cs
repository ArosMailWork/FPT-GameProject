using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Melee,
    Shooter
}

public class MoveToPlayer : BehaviorNode
{
    [SerializeField] EnemyType enemyType;
    [SerializeField] NavMeshAgent navMeshAgent;
    public Transform transform, player;
    public float stoppingDistance = 1f, attackRange = 0.3f;
    public float minMoveDis = 5f, maxMoveDis = 7f;
    public float approachAngle = 90f;
    public bool tryMoveBehind;
    
    //Calculate
    private float randomAngle, randomDistance;
    private Vector3 randomDirection, directionToPlayer, randomPosition, targetPosition;
    private Quaternion rotation;

    public override NodeState Evaluate()
    {
        if (player == null)
        {
            state = NodeState.Failure;
            return state;
        }

        // Shooter Strafe
        if (enemyType == EnemyType.Shooter)
        {
            randomPosition =
                tryMoveBehind ? GetRandomPositionBehindPlayer() : GetRandomPositionInFrontOfPlayer();
            navMeshAgent.SetDestination(randomPosition);
        }
        else if (enemyType == EnemyType.Melee)
        {
            // Move to the player but keep attack range
            directionToPlayer = (player.position - transform.position).normalized;
            targetPosition = player.position - directionToPlayer * attackRange;
            navMeshAgent.SetDestination(targetPosition);
        }

        // Check if the agent is within stopping distance of the player
        if (Vector3.Distance(transform.position, player.position) <= stoppingDistance)
        {
            state = NodeState.Success;
            return state;
        }

        // If the agent is still moving towards the player
        if (navMeshAgent.pathPending || navMeshAgent.remainingDistance > stoppingDistance)
        {
            state = NodeState.Running;
            return state;
        }

        // If the agent has reached the destination
        state = NodeState.Success;
        return state;
    }

    private Vector3 GetRandomPositionBehindPlayer()
    {
        // Calculate the direction from the player to the enemy
        directionToPlayer = (transform.position - player.position).normalized;
        // Calculate a random angle within -90 to +90 degrees (180 degrees total)
        randomAngle = Random.Range(-90f, 90f);
        rotation = Quaternion.Euler(0, randomAngle, 0);
        
        // Calculate the random position behind the player within the range of 5 to 7 meters
        randomDistance = Random.Range(minMoveDis, maxMoveDis);
        randomDirection = rotation * directionToPlayer;
        return player.position + randomDirection * randomDistance;
    }

    private Vector3 GetRandomPositionInFrontOfPlayer()
    {
        // Calculate the direction from the player to the enemy
        directionToPlayer = (player.position - transform.position).normalized;
        // Calculate a random angle within -90 to +90 degrees (180 degrees total)
        randomAngle = Random.Range(-90f, 90f);
        rotation = Quaternion.Euler(0, randomAngle, 0);
        // Calculate the random position in front of the player within the range of 5 to 7 meters
        randomDistance = Random.Range(minMoveDis, maxMoveDis);
        randomDirection = rotation * directionToPlayer;
        return player.position + randomDirection * randomDistance;
    }
}