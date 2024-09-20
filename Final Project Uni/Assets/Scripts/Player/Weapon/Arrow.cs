using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum ArrowState
{
    Idle,
    Shooting,
    Recalling
}

public class Arrow : MonoBehaviour
{
    public ArrowState currentArrowState;
    public MeshRenderer arrowMeshRenderer;
    [FoldoutGroup("Stats")] public float lifeTime, recallSpeed, rotSpeed = 10, MaxSpeed;
    [FoldoutGroup("Stats/Hover")] public float hoverSpeed = 2.0f;

    [FoldoutGroup("Debug")] [ReadOnly] public float currentLifeTime;
    [FoldoutGroup("Debug")] public Vector3 RecallDirect;
    [FoldoutGroup("Debug/Hover")] public float currentHoverHeight;

    [FoldoutGroup("Setup")] public Rigidbody arrowRb;
    [FoldoutGroup("Setup")] [ReadOnly] public ArrowController _arrowController;
    [FoldoutGroup("Setup")] [ReadOnly] public PlayerController _playerController;
    [FoldoutGroup("Setup/Events")] public UnityEvent StartRecallEvent, StopRecallEvent, FinishRecallEvent;

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

        //todo: maybe do shoot for windows input
        // if (currentArrowState == ArrowState.Shooting)
        // {
        //     Shoot(_arrowController.shootDirection);
        // }


        if (isAttached)
        {
            transform.position = _playerController.transform.position;
        }

        // Rotate the arrow to point in the direction of its velocity
        if (arrowRb.velocity.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(arrowRb.velocity.normalized);
            arrowRb.rotation =
                Quaternion.Slerp(arrowRb.rotation, targetRotation,
                    Time.deltaTime * rotSpeed); // Adjust the speed of rotation here
        }
    }

    #endregion

    public void AssignController()
    {
        _arrowController = ArrowController.Instance;
        _playerController = PlayerController.Instance;
    }

    #region Shooting

    [Button]
    public void Shoot(float force)
    {
        //todo: maybe rotate with player
        isAttached = false;

        // Set the arrow's Rigidbody back to non-kinematic
        arrowRb.isKinematic = false;

        //just disable visibilty
        if (arrowMeshRenderer.enabled == false)
        {
            arrowMeshRenderer.enabled = true;
        }

        arrowRb.AddForce(_playerController.transform.forward * force, ForceMode.Impulse);
    }

    bool isAttached = false;

    private void OnTriggerEnter(Collider other)
    {        
        if (currentArrowState == ArrowState.Shooting) return;
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Recover");
            _playerController.currentState = PlayerState.Idle;
            _arrowController.haveArrow = true;
            _arrowController.isRecalling = false;
            currentArrowState = ArrowState.Idle;
            arrowRb.velocity = Vector3.zero;
            arrowMeshRenderer.enabled = false;

            isAttached = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _arrowController.haveArrow = false;
            isAttached = false;
        }
    }

    #endregion

    #region Recalling

    public void Recall()
    {
        RecallDirect = _playerController.transform.position - transform.position;
        DragArrow();
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
}