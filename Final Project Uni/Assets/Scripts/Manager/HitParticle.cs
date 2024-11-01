using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticle : MonoBehaviour
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
        
        //Implement Knockback shiet here
        KnockDirect.y = 0;
        AudioManager.Instance.PlaySoundEffect("dash");
    }

    public void PlayExplodeParticle()
    {
        ParticleManager.Instance.SpawnParticle("BotExplode",pivot.position, Quaternion.Euler(0, 0, 0));
    }
    
}
