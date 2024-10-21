using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemyTest : MonoBehaviour
{

    public Transform pivot;

    public void Start()
    {
        if(pivot == null)
            pivot = transform;
    }
    public void SpawnKnockbackParticle(Vector3 KnockDirect)
    {
        doSpawnKnockbackParticle(KnockDirect);
    }

    public async UniTaskVoid doSpawnKnockbackParticle(Vector3 KnockDirect, float StunTime = 0.15f)
    {
        GameObject prefab =  ParticleManager.Instance.SpawnParticle("HitEffect", 
            pivot.position
            , Quaternion.LookRotation(KnockDirect));
        //Debug.Log("Player Knockback: " + KnockDirect);
        //Implement Knockback shiet here
        KnockDirect.y = 0;

    }
}
