using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public float speed = 35f;

    private HurtBox _hurtBox;
    List<InteractTarget> validTargets = new List<InteractTarget>(); // List of valid targets

    private void Awake()
    {
        _hurtBox = GetComponent<HurtBox>();
        _hurtBox.isProjectile = true;
        validTargets = _hurtBox.validTargets;
    }

    public void OnEnable()
    {
        rb.transform.rotation = Quaternion.Euler(0, rb.transform.rotation.eulerAngles.y, 0);
        
        // Set the velocity along the X and Z axes while keeping Y velocity zero to ensure the projectile stays level with the ground
        Vector3 forwardVelocity = transform.forward * speed;
        rb.velocity = new Vector3(forwardVelocity.x, 0, forwardVelocity.z); // Y velocity is set to 0
        
        //_hurtBox.KnockDir = rb.velocity; // KnockDir is also set to this velocity
    }

    public void SelfDestruct()
    {
        //Can add particle or sth here
        PoolManager.Instance.Despawn(gameObject);
    }
}
