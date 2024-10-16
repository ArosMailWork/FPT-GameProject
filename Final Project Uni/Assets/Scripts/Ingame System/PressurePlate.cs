using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public enum InteractTarget
    {
        Player, Arrow, Enemy
    }

    // List of targets to check against
    public List<InteractTarget> validTargets = new List<InteractTarget>();

    public UnityEvent ToggleOn, ToggleOff;
    public bool pressing;

    // List to track objects currently inside the trigger
    private List<Collider> objectsInTrigger = new List<Collider>();

    // Function to check if the tag matches any in the validTargets list
    private bool IsValidTarget(Collider other)
    {
        // If no specific target is provided, we assume all tags are valid
        if (validTargets.Count == 0)
            return true;

        // Check if the tag matches any of the valid targets
        foreach (InteractTarget target in validTargets)
        {
            if (other.CompareTag(target.ToString()))
                return true;
        }

        return false; // No matching tag found
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsValidTarget(other))
            return; // Ignore if not a valid target

        // Add object to the list if not already present
        if (!objectsInTrigger.Contains(other))
        {
            objectsInTrigger.Add(other);
            pressing = true;
            ToggleOn.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsValidTarget(other))
            return; // Ignore if not a valid target

        // Remove the object from the list when it exits
        if (objectsInTrigger.Contains(other))
        {
            objectsInTrigger.Remove(other);
            pressing = false;
            ToggleOff.Invoke();
        }
    }

    private void Update()
    {
        // Iterate over objects in the trigger to check if any are disabled
        for (int i = objectsInTrigger.Count - 1; i >= 0; i--)
        {
            if (objectsInTrigger[i] == null || !objectsInTrigger[i].gameObject.activeInHierarchy)
            {
                // If object is disabled or destroyed, invoke ToggleOff and remove it from the list
                objectsInTrigger.RemoveAt(i);
                pressing = false;
                ToggleOff.Invoke();
            }
        }
    }
}
