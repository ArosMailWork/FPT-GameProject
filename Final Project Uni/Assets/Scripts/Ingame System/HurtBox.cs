using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    #region Variables
    
    public bool Activate = true;
    [CanBeNull] public Transform customPivot;
    
    [FoldoutGroup("Stats")]
    public List<InteractTarget> validTargets = new List<InteractTarget>(); // List of valid targets

    [FoldoutGroup("Stats/Damage")]
    public int BaseDamage;
    [FoldoutGroup("Stats/Damage")]
    public float DamageMultiplier = 1, MirageMultiplier = 1;
    [FoldoutGroup("Stats/Damage")]
    public float DotTime;
    [FoldoutGroup("Stats/Knockback")]
    public bool doKnockback;
    [FoldoutGroup("Stats/Knockback")]
    public float KnockForce;
    
    [FoldoutGroup("Stats/Knockback")]
    public Vector3 KnockDir; // Modify this by other component
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private bool dotDam;
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private Vector3 direction;

    // Track already hit objects
    [FoldoutGroup("Debug")]
    private HashSet<Collider> hitObjects = new HashSet<Collider>();
    
    RaycastHit hitInfo;
    private Vector3 pivotPosition;

    #endregion
    
    #region Unity Methods
    
    private void Awake()
    {
        dotDam = (BaseDamage > 1 && DotTime > 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Activate || !IsValidTarget(other) || hitObjects.Contains(other)) return;

        // Use customPivot if it's set, otherwise use transform.position
        pivotPosition = customPivot != null ? customPivot.position : transform.position;
        KnockDir = other.transform.position - pivotPosition;
        
        Debug.DrawRay(pivotPosition, KnockDir * 10, Color.green, 2.0f);

        if (other.TryGetComponent<Health>(out Health targetHealth))
        {
            HitTarget(targetHealth, KnockDir);
            hitObjects.Add(other); // Mark the object as hit
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Apply the same logic for objects already inside the HurtBox
        if (!Activate || !IsValidTarget(other) || hitObjects.Contains(other)) return;

        // Use customPivot if it's set, otherwise use transform.position
        pivotPosition = customPivot != null ? customPivot.position : transform.position;
        KnockDir = other.transform.position - pivotPosition;

        Debug.DrawRay(pivotPosition, KnockDir * 10, Color.green, 2.0f);

        if (other.TryGetComponent<Health>(out Health targetHealth))
        {
            HitTarget(targetHealth, KnockDir);
            hitObjects.Add(other); // Mark the object as hit
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the object from hitObjects when it leaves the HurtBox, so it can be hit again if it re-enters
        if (!hitObjects.Contains(other)) return;
        
        hitObjects.Remove(other);
    }

    #endregion

    // Check if the target tag is in the valid targets list
    private bool IsValidTarget(Collider other)
    {
        if (validTargets.Count == 0) return true;

        foreach (InteractTarget target in validTargets)
        {
            if (other.CompareTag(target.ToString())) return true;
        }

        return false; // No matching tag found
    }

    public void ToggleHurtBox(bool toggle)
    {
        Activate = toggle;
    }

    void HitTarget(Health targetHealth, Vector3? knockDir = null)
    {
        int Damage = Mathf.CeilToInt(BaseDamage * DamageMultiplier * MirageMultiplier);
        
        if (dotDam) 
            targetHealth.DamageOverTime(Damage, DotTime);
        else 
            targetHealth.Hurt(Damage);

        direction = knockDir ?? Vector3.zero;

        if (doKnockback && KnockForce > 0 && direction != Vector3.zero)
            targetHealth.Knockback(direction.normalized, KnockForce);

        KnockDir = Vector3.zero; // Reset knockback direction
    }
}
