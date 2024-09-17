using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class temp : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public bool isUpdate;
    public float force;
    public ForceMode forceMode;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    [Button]
    public void AddForce()
    {
        rb.AddForce(transform.forward * force, forceMode);
    }
    [Button]
    public void ResetX()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isUpdate)
        {
            rb.AddForce(transform.forward * force, forceMode);
        }
    }
}