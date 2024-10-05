using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    private bool waiting;
    [SerializeField] public float stopDuration;

    public void Stop()
    {
        if (waiting) return;
        
        StartCoroutine(Wait(stopDuration));
    }

    public IEnumerator Wait(float duration)
    {
        Time.timeScale = 0f;
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        waiting = false;
    }
}
