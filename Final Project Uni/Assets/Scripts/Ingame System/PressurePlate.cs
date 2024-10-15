using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//use this for activate shiets
public class PressurePlate : MonoBehaviour
{
    public UnityEvent ToggleOn, ToggleOff, Holding;
    public bool pressing;
    
    private void OnTriggerEnter(Collider other)
    {
        pressing = true;
        ToggleOn.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        Holding.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        pressing = false;
        ToggleOff.Invoke();
    }
}
