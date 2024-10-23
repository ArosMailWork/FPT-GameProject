using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AimType
{
    Basic, Predict, AccuratePredict
}

public class BotGun : MonoBehaviour
{
    public AimType aimType;
    public GameObject projectile;
    public Rigidbody target;
    public float predictionFactor = 0.5f;
    public float projectileSpeed = 35f;

    private Rigidbody projectileRB;

    public void Fire()
    {
        switch (aimType)
        {
            case AimType.Basic:
                FireStraight();
                break;
            case AimType.Predict:
                FirePredict();
                break;
            case AimType.AccuratePredict:
                FirePredictHighAccurate();
                break;
        }
    }
    
    // Regular fire method (straight shooting)
    public void FireStraight()
    {
        // Get the world space rotation directly (without local rotation adjustments)
        Quaternion worldRotation = Quaternion.LookRotation(transform.forward, Vector3.up);

        // Spawn the projectile with the world space rotation
        PoolManager.Instance.Spawn(projectile, transform.position, worldRotation);
    }

    // predictive shooting method
    public void FirePredict()
    {
        if (target == null) return;

        // Get the target's velocity and predict the future position based on that velocity
        Vector3 targetVelocity = target.velocity;
        Vector3 predictedPosition = target.position + new Vector3(targetVelocity.x, 0, targetVelocity.z) * predictionFactor;

        // Calculate the direction to the predicted position (ignoring Y-axis for horizontal shots)
        Vector3 targetDirection = predictedPosition - transform.position;
        targetDirection.y = 0; // Ensure the projectile stays parallel to the ground

        // Calculate the Y-axis rotation (horizontal plane only)
        Vector3 flattenedDirection = new Vector3(targetDirection.x, 0, targetDirection.z); // Flatten the direction to ignore Y-axis
        Quaternion predictedRotation = Quaternion.LookRotation(flattenedDirection, Vector3.up); // Look at the target only on the Y-axis


        // Spawn the projectile with the predicted rotation
        PoolManager.Instance.Spawn(projectile, transform.position, predictedRotation);

        // Apply velocity to the projectile to ensure it flies towards the predicted position
        projectileRB = projectile.GetComponent<Rigidbody>();
        if (projectileRB != null)
        {
            projectileRB.velocity = predictedRotation * Vector3.forward * projectileSpeed;
        }
    }
    
    public void FirePredictHighAccurate()
    {
        if (target == null) return;

        // Get the target's velocity
        Vector3 targetVelocity = target.velocity;

        // Calculate the distance to the target
        Vector3 distanceToTarget = target.position - transform.position;
        float distanceMagnitude = distanceToTarget.magnitude;

        // Calculate the time it will take for the projectile to reach the target's current position
        float timeToReachTarget = distanceMagnitude / projectileSpeed;

        // Predict the future position of the target considering the time it takes for the projectile to reach them
        Vector3 predictedPosition = target.position + (targetVelocity * predictionFactor) * timeToReachTarget;

        // Calculate the direction to the predicted position (ignoring Y-axis for horizontal shots)
        Vector3 targetDirection = predictedPosition - transform.position;
        targetDirection.y = 0; // Ensure the projectile stays parallel to the ground

        // Calculate the Y-axis rotation (horizontal plane only)
        Vector3 flattenedDirection = new Vector3(targetDirection.x, 0, targetDirection.z); // Flatten the direction to ignore Y-axis
        Quaternion predictedRotation = Quaternion.LookRotation(flattenedDirection, Vector3.up); // Look at the target only on the Y-axis

        // Spawn the projectile with the predicted rotation
        PoolManager.Instance.Spawn(projectile, transform.position, predictedRotation);

        // Apply velocity to the projectile to ensure it flies towards the predicted position
        projectileRB = projectile.GetComponent<Rigidbody>();
        if (projectileRB != null)
        {
            // Set the projectile's velocity towards the predicted position
            projectileRB.velocity = predictedRotation * Vector3.forward * projectileSpeed;
        }
    }

}
