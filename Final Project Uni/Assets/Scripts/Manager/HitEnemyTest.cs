using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemyTest : MonoBehaviour
{
    public Vector3 offset;
    public Rigidbody rb;

    // Use OnTriggerEnter instead of OnCollisionEnter
    private void OnTriggerEnter(Collider objectWeHit)
    {
        if (objectWeHit.CompareTag("Target"))
        {
            Debug.Log("hit " + objectWeHit.gameObject.name + "!");
            CreateParticle(objectWeHit);
            //Destroy(gameObject);
        }
    }

    private void CreateParticle(Collider objectWeHit)
    {
        // Get the closest point of impact on the collider
        Vector3 hitPoint = objectWeHit.ClosestPoint(transform.position);

        // Calculate the normal direction from the collider's center to the hit point
        Vector3 hitNormal = (hitPoint - objectWeHit.bounds.center).normalized;

        // Adjust the rotation of the particle to align with the calculated normal
        Quaternion rotation = Quaternion.LookRotation(hitNormal);

        // Spawn particle at the hit point with the calculated normal
        GameObject particlePrefab = ParticleManager.Instance.SpawnOppositeParticle
            (ParticleManager.Instance.prefab, hitPoint, rotation);

        // Offset the particle position slightly
        particlePrefab.transform.position = hitPoint + offset;
    }
}