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

    private void Awake()
    {
        _hurtBox = GetComponent<HurtBox>();
        _hurtBox.isProjectile = true;
    }

    public void Start()
    {
        rb.velocity = transform.forward * speed;
        _hurtBox.KnockDir = rb.velocity;
    }
}
