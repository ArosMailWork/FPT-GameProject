using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    //public Rigidbody rb;
    float speed = 25f;
    public void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


}
