using System;
using System.Collections;
using System.Collections.Generic;
using Mono.CSharp;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public enum HitTag
{
    Player, Enemy
}

public class HurtBox : MonoBehaviour
{
    #region Variables
    
    public bool Activate = true;
    [FoldoutGroup("Stats")]
    public HitTag TargetTag;
    [FoldoutGroup("Stats/Damage")]
    public int Damage;
    [FoldoutGroup("Stats/Damage")]
    public float DotTime;
    
    [FoldoutGroup("Stats/Knockback")]
    public bool doKnockback;
    [FoldoutGroup("Stats/Knockback")]
    public float KnockForce;
    [FoldoutGroup("Stats/Knockback")]
    public Vector3 KnockDir; //modify this by other component

    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private bool dotDam;
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private Vector3 direction;
    
    #endregion
    
    #region Unity Methods
    
    private void Awake()
    {
        dotDam = (Damage > 1 && DotTime > 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!Activate || other.gameObject.tag != TargetTag.ToString()) return;
        
        if (other.TryGetComponent<Health>(out Health targetHealth))
            HitTarget(targetHealth, KnockDir);
    }

    #endregion

    public void ToggleHurtBox(bool toggle)
    {
        Activate = toggle;
    }
    
    void HitTarget(Health targetHealth, Vector3? knockDir = null)
    {
        //if(!Activate) return;
        
        //check DoT mode or not
        if(dotDam) targetHealth.DamageOverTime(Damage, DotTime);
        else targetHealth.Hurt(Damage);
        
        //Knock
        direction = knockDir ?? Vector3.zero;
        if (doKnockback && KnockForce <= 0 && direction != Vector3.zero) 
            targetHealth.Knockback(direction.normalized, KnockForce);
        
        //Reset
        KnockDir = Vector3.zero;
    }
}
