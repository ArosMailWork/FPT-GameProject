using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossSkill : MonoBehaviour
{
    public int phase = 0;
    
    [FoldoutGroup("Setup")]
    public List<BotGun> guns;
    [FoldoutGroup("Setup")] 
    [CanBeNull] public GameObject Striking;
    
    [FoldoutGroup("Setup")] 
    public int RNG_Min = 0, RNG_Max = 5;
    [FoldoutGroup("Setup")] 
    public float offset = 3;

    [FoldoutGroup("Setup/Pattern Pos")] 
    public bool patternBossAttack, patternBossMove;
    [FoldoutGroup("Setup/Pattern Pos")] 
    public List<Vector3> PatternPos;

    [FoldoutGroup("Debug")] 
    [ReadOnly] public int RNG;
    [FoldoutGroup("Debug")] 
    [ReadOnly] public Rigidbody target;

    [HideInInspector] public BotSM sm;
    
    public void Attack(Rigidbody rb)
    {
        SetTarget(rb);

        switch (phase)
        {
            case 0:
                FirstPhase(target);
                break;
            case 1:
                SecondPhase(target);
                break;
        }
    }

    public void SetTarget(Rigidbody rb)
    {
        if(target != null) return;
        
        target = rb;
        foreach (var gun in guns)
        {
            if(gun != null) gun.target = target;
        }
    }

    public void FirstPhase(Rigidbody target)
    {
        if(sm.bot._animController != null)
            sm.bot._animController.AttackAnim(true);

        if (RNG >= RNG_Max - 2)
        {
            if(sm.bot._animController != null)
                sm.bot._animController.SpecialAttackAnim(true, 0);
        }
        
        SpinRNG();
    }
    
    public void SecondPhase(Rigidbody rb)
    {
        
    }

    public void doTeleport(Vector3 pos)
    {
        
    }
    
    public void doTeleportBehindPlayer()
    {
        sm.StrafeState.MoveToBehindPlayer(3f);
    }
    
    public void doSummonStrikingX()
    {
        SummonStrikingInXPattern(target.transform.position, offset);
    }
    
    public void SummonStriking(Vector3 summonPos, Vector3 offset = default)
    {
        if (offset == default) offset = Vector3.zero;

        Instantiate(Striking, summonPos + offset, Quaternion.identity);
    }
    // Method to summon in an "X" pattern
    void SummonStrikingInXPattern(Vector3 summonPos, float offsetDistance)
    {
        // Calculate positions in an "X" pattern relative to the central summon position
        Vector3 offset1 = new Vector3(offsetDistance, 0, offsetDistance);
        Vector3 offset2 = new Vector3(-offsetDistance, 0, offsetDistance);
        Vector3 offset3 = new Vector3(offsetDistance, 0, -offsetDistance);
        Vector3 offset4 = new Vector3(-offsetDistance, 0, -offsetDistance);

        // Summon at each of the offset positions
        SummonStriking(summonPos, Vector3.zero);
        SummonStriking(summonPos, offset1);
        SummonStriking(summonPos, offset2);
        SummonStriking(summonPos, offset3);
        SummonStriking(summonPos, offset4);
        
        SpinRNG();
    }

    void SpinRNG()
    {
        RNG = Random.Range(RNG_Min, RNG_Max);
    }
}
