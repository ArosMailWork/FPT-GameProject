using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FixPosition : MonoBehaviour
{
    public Vector3 initialPosition = new Vector3(900, 540, 0);
    public Vector3 initialLocalScale = Vector3.one;
    public Vector3 debugPosition;
    public Vector3 debugScale;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = initialPosition;
    }

    void OnEnable()
    {
        initialLocalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = initialPosition;
        debugPosition = transform.position;

        if (!transform.parent) return;
        transform.localScale = new Vector3(
            initialLocalScale.x / transform.parent.lossyScale.x,
            initialLocalScale.y / transform.parent.lossyScale.y,
            initialLocalScale.z / transform.parent.lossyScale.z
        );
        //print($"Lc: {transform.localScale} and Gb: {transform.parent.lossyScale}");
        debugScale = transform.localScale;
    }
}