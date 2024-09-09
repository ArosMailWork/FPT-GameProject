using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = System.Numerics.Vector3;

[Serializable]
public struct DataPack
{
    public Vector3 pos;
    public Vector3 rot;
    public float hp;
    public float mp;
    public float stamina;
}

public class HealthTest : MonoBehaviour
{
    public float health, maxHealth, currentHealth;

    
    public UnityEvent eventTest;
    public UnityEvent<Vector3> eventTest2;
    public UnityEvent<DataPack> eventTest3;

    private void Awake()
    {
    }

    [Button]
    public void Hurt()
    {
        DataPack dp = new DataPack()
        {
            pos = Vector3.One,
            rot = Vector3.One,
            hp = 100,
            mp = 50,
            stamina = 20,
        };
        
        health -= 2;
        Knockback(dp);
    }

    private void Knockback(DataPack dp)
    {
        Vector3 vecTest = Vector3.One;
        
        eventTest.Invoke();
        eventTest2.Invoke(vecTest);
        eventTest3.Invoke(dp);
    }
}
