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
    public float lifeTime, recallSpeed, rotSpeed = 10, MaxSpeed, minShootingSpeed = 0.5f, MirageDelay = 0.5f;
    [FoldoutGroup("Stats/Hover")]
    public float hoverSpeed = 2.0f;

    [FoldoutGroup("Debug")]
    [ReadOnly] public bool RecallBuffer;
    [FoldoutGroup("Debug")]
    [ReadOnly] public float CurrentVelocity;
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
    public float offset;
    public bool IsMainArrow;

    #region Unity Methods

    private void Awake()
    {
        arrowRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        AssignController();
    }

    private void FixedUpdate()
    {
        CurrentVelocity = arrowRb.velocity.magnitude;
    }

    private void Update()
    {
        if (currentArrowState == ArrowState.Shooting && arrowRb.velocity.magnitude <= minShootingSpeed)
        {
            //Debug.Log("Idle");
            currentArrowState = ArrowState.Idle;
        }

        if (currentArrowState == ArrowState.Recalling)
            Recall();

        else if (currentArrowState == ArrowState.Idle)
        {
            currentHoverHeight = 0;
            if (RecallBuffer) currentArrowState = ArrowState.Recalling;
        }

        // Rotate the arrow to point in the direction of its velocity
        if (arrowRb.velocity.magnitude > 0 && arrowRb.velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(arrowRb.velocity.normalized);
            arrowRb.rotation = Quaternion.Slerp(arrowRb.rotation, targetRotation, Time.deltaTime * rotSpeed); // Adjust the speed of rotation here
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
        //Debug.Log(AccelDirect);
        arrowRb.velocity = AccelDirect;
    }

    #region Recalling

    public void Recall()
    {
        RecallBuffer = false;
        RecallDirect = _playerController.transform.position - transform.position;
        DragArrow();
    }

    void DragArrow()
    {
        arrowRb.AddForce(RecallDirect.normalized * (recallSpeed * Time.fixedDeltaTime * 240), ForceMode.Acceleration);
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

        //hit anything -> allow recall
        if (currentArrowState == ArrowState.Shooting) return;
        if (other.gameObject.tag == "Player")
        {
            if (IsMainArrow)
            {
                _playerController.currentState = PlayerState.Idle;
                _arrowController.haveArrow = true;
                _arrowController._playerAnimController.UpdateHaveArrow(true);
                _arrowController.isRecalling = false;
                
                if(_arrowController.ShootButtonPressing)
                    _arrowController.arrowRecoverFlag = true;
                
                _arrowController.HideAllArrow(MirageDelay);
                currentArrowState = ArrowState.Idle;
            }
            HideArrow();

        }
    }
    public void HideArrow()
    {
        //can Play an Event here
        
        
        //hide and deactivate it
        arrowRb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        //hit anything -> allow recall
        if (currentArrowState == ArrowState.Shooting) currentArrowState = ArrowState.Idle;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            _arrowController.haveArrow = false;
    }

}
