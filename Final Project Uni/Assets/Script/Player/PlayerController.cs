using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public Vector3 movement;
    void Start()
    {

    }
    void FixedUpdate()
    {
        moveCharacter(movement);
    }
    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
    }
    void moveCharacter(Vector3 direction)
    {
        rb.velocity = direction * speed;
    }
}
