using System;
using DG.Tweening;
using UnityEngine;

public class Example : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter called with: {other.gameObject.name}");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player tag detected");
        }
        else
        {
            Debug.Log($"Collider entered, but not player. Tag: {other.tag}");
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"OnCollisionEnter called with: {other.gameObject.name}");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player tag detected");
        }
        else
        {
            Debug.Log($"Collider entered, but not player. Tag: {other.gameObject.tag}");
        } }

    private void Update()
    {
        print("example");
    }
}