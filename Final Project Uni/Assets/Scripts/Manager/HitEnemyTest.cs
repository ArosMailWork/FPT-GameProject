using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemyTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision objectWeHit)
    {
        if (objectWeHit.gameObject.CompareTag("Target"))
        {
            Debug.Log("hit" + objectWeHit.gameObject.name + "!");
            CreateParticle(objectWeHit);
            //Destroy(gameObject);
        }
    }

    private void CreateParticle(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject particlePrefab = ParticleManager.Instance.SpawnParticle
                (ParticleManager.Instance.particlePrefabs[0], contact.point, Quaternion.LookRotation(contact.normal));

        particlePrefab.transform.SetParent(objectWeHit.gameObject.transform);
    }
}
