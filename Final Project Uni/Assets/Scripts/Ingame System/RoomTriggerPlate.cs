using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomTriggerPlate : MonoBehaviour
{
    // List of targets to check against
    public List<InteractTarget> validTargets = new List<InteractTarget>();

    public UnityEvent ToggleOn, ToggleOff;
    public float DelayActivateTime, DelayLeftTime;
    public bool pressing;


    // Function to check if the tag matches any in the validTargets list
    private bool IsValidTarget(Collider other)
    {
        // If no specific target is provided, we assume all tags are valid
        if (validTargets.Count == 0) return true;

        // Check if the tag matches any of the valid targets
        foreach (InteractTarget target in validTargets)
        {
            if (other.CompareTag(target.ToString())) return true;
        }

        return false; // No matching tag found
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered");
        if (!IsValidTarget(other)) return;
        TriggerToggle(true);
    }

    [Button]
    public async UniTaskVoid TriggerToggle(bool toggle)
    {
        if (toggle)
        {
            pressing = toggle;
            await UniTask.Delay(TimeSpan.FromSeconds(DelayActivateTime));
            ToggleOn.Invoke();
        }
        else if (!toggle)
        {
            pressing = true;
            await UniTask.Delay(TimeSpan.FromSeconds(DelayLeftTime));
            ToggleOff.Invoke();
        }

    }
}
