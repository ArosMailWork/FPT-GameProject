using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum ArrowState
{
    Idle, Shooting, Recalling
}

public class Arrow : MonoBehaviour
{
    public ArrowState currentArrowState;

    [FoldoutGroup("Stats")]
    public Vector3 AccelDirect;
    [FoldoutGroup("Stats")]
    public float lifeTime, recallSpeed, recallRotTime = 0.1f, MaxSpeed;
    [FoldoutGroup("Stats/Hover")]
    public float hoverSpeed = 2.0f;
    
    [FoldoutGroup("Debug")]
    [ReadOnly] public float currentLifeTime;
    [FoldoutGroup("Debug")]
    public Vector3 RecallDirect;
    [FoldoutGroup("Debug/Hover")]
    public float currentHoverHeight; 

    [FoldoutGroup("Setup")] 
    public Rigidbody arrowRb;
    [FoldoutGroup("Setup")] 
    [ReadOnly] public ArrowController _arrowController;
    [FoldoutGroup("Setup")] 
    [ReadOnly] public PlayerController _playerController;
    [FoldoutGroup("Setup/Events")]
    public UnityEvent StartRecallEvent, StopRecallEvent, FinishRecallEvent;
    

    #region Unity Methods

    private void Awake()
    {
        arrowRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        AssignController();
    }

    private void Update()
    {
        if (currentArrowState == ArrowState.Recalling)
            Recall();
        else if (currentArrowState == ArrowState.Idle)
        {
            currentHoverHeight = 0;
        }
    }

    #endregion
    public void AssignController()
    {
        _arrowController = ArrowController.Instance;
        _playerController = PlayerController.Instance;
    }
    

    public void Shoot(Vector3 inputDirect)
    {
        AccelDirect = inputDirect;
    }

    #region Recalling
    
    public void Recall()
    {
        RecallDirect = _playerController.transform.position - transform.position;
        StartCoroutine(RotateArrowCoroutine(RecallDirect));
        DragArrow();
    }
    
    private IEnumerator RotateArrowCoroutine(Vector3 moveDirection)
    {
        // Calculate the target rotation based only on the Y and Z axes
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        // Preserve the current X rotation
        targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        float elapsedTime = 0f;
        Quaternion initialRotation = arrowRb.rotation;
        while (elapsedTime < recallRotTime)
        {
            elapsedTime += Time.deltaTime;
            arrowRb.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / recallRotTime);
            yield return null; // Wait until the next frame
        }
        // Ensure the final rotation is set to the target rotation
        arrowRb.rotation = targetRotation;
    }
    void DragArrow()
    {
        arrowRb.AddForce(RecallDirect.normalized * recallSpeed, ForceMode.Acceleration);
        LimitSpeed();
    }
    
    
    #endregion
    
    void LimitSpeed()
    {
        Mathf.Clamp(arrowRb.velocity.magnitude, 0, MaxSpeed);
        if (arrowRb.velocity.magnitude > MaxSpeed)
            arrowRb.velocity = arrowRb.velocity.normalized * MaxSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Recover");
            _playerController.currentState == PlayerState.Idle;
        }
    }
}
