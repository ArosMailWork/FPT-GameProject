using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ReviveCountdownTimer : MonoBehaviour
{
    [FoldoutGroup("Settings")]
    [CanBeNull] public TMP_Text countdownText;
    [FoldoutGroup("Settings")]
    public int startTimeInSeconds = 10;
    [FoldoutGroup("Settings/Events")]
    public UnityEvent OnCountdownComplete; 
    
    [FoldoutGroup("Debug")]
    public bool isRunning = false;

    private CancellationTokenSource cts;

    private void OnEnable()
    {
        StartCountdown();
    }

    [Button]
    public void StartCountdown()
    {
        if (isRunning) return; // Prevent multiple countdowns at the same time
        cts = new CancellationTokenSource();
        CountdownAsync(cts.Token).Forget();
    }
    [Button]
    public void StopCountdown()
    {
        if (cts != null)
        {
            cts.Cancel(); // Cancel the ongoing task
            cts.Dispose();
            cts = null;
        }
        isRunning = false;
    }

    public void EnableDelay(float delay)
    {
        doEnable(delay);
    }
    
    async UniTaskVoid doEnable(float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        gameObject.SetActive(true);
    }
    
    async UniTaskVoid CountdownAsync(CancellationToken cancellationToken)
    {
        isRunning = true;

        int currentTime = startTimeInSeconds;
        while (currentTime > 0)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                isRunning = false;
                return; // Exit if cancelled
            }

            // Update TMP text to display the countdown
            if(countdownText != null)
            countdownText.text = currentTime.ToString();

            // Wait for 1 second asynchronously
            await UniTask.Delay(1000, cancellationToken: cancellationToken);

            // Decrease the time
            currentTime--;
        }

        // Ensure the countdown ends at 0
        if(countdownText != null)
        countdownText.text = "0";

        // Invoke the Unity Event
        OnCountdownComplete?.Invoke();

        isRunning = false;
    }
}
