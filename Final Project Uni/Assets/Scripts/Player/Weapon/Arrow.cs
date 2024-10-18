using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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

    [FoldoutGroup("Stats")] 
    public float bonusRicochetMultiplier = 0;
    [FoldoutGroup("Stats/Hover")]
    public float hoverSpeed = 2.0f;

    [FoldoutGroup("Debug")]
    [ReadOnly] public float CurrentVelocity;
    [FoldoutGroup("Debug")]
    public Vector3 RecallDirect;
    [FoldoutGroup("Debug/Hover")]
    public float currentHoverHeight;

    [FoldoutGroup("Setup")]
    public Rigidbody arrowRb;
    [FoldoutGroup("Setup")] 
    public HurtBox hitbox;
    [FoldoutGroup("Setup")]
    [ReadOnly] public ArrowController _arrowController;
    [FoldoutGroup("Setup")]
    [ReadOnly] public PlayerController _playerController;
    [FoldoutGroup("Setup/Events")]
    public UnityEvent StartRecallEvent, StopRecallEvent, RecoverEvent, HideEvent;
    public float offsetDegree;
    public bool IsMainArrow;

    #region Unity Methods

    private void Start()
    {
        _arrowController = ArrowController.Instance;
        _playerController = PlayerController.Instance;
        
        RecoverEvent.Invoke();
        if (!IsMainArrow) hitbox.MirageMultiplier = 0.5f;
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

        if (currentArrowState == ArrowState.Recalling) Recall();

        else if (currentArrowState == ArrowState.Idle)
        {
            currentHoverHeight = 0;
            
            if (IsMainArrow && currentArrowState == ArrowState.Recalling)
                _arrowController.prefabParticleManager.PlayAssignedParticle("RecallingMainArrowVFX");
        }

        // Rotate the arrow to point in the direction of its velocity
        if (arrowRb.velocity.magnitude > 0 && arrowRb.velocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(arrowRb.velocity.normalized);
            arrowRb.rotation = Quaternion.Slerp(arrowRb.rotation, targetRotation, Time.deltaTime * rotSpeed); // Adjust the speed of rotation here
        }
    }

    private void OnDisable()
    {
        //HideEvent.Invoke();

        _arrowController.prefabParticleManager.SpawnParticle("ArrowHideVFX", 
            transform.position, Quaternion.Euler(-90, 0, 0));
    }

    #endregion
    public void Assign()
    {
        arrowRb = GetComponent<Rigidbody>();
        hitbox = GetComponent<HurtBox>();
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
        RecallDirect = _playerController.transform.position - transform.position;
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

    private void OnTriggerStay(Collider other)
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
                
                if(_arrowController.IsSplitShot)
                    _arrowController.HideAllMirageArrow(MirageDelay);
                currentArrowState = ArrowState.Idle;
            }
            HideArrow();
        }
    }
    public void HideArrow()
    {
        //can Play an Event here
        RecoverEvent.Invoke();
        
        //hide and deactivate it
        if(!gameObject.activeSelf) return;
            
        arrowRb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        //hit anything -> allow recall
        if (currentArrowState == ArrowState.Shooting) currentArrowState = ArrowState.Idle;

        if(bonusRicochetMultiplier > 0)
        arrowRb.velocity = arrowRb.velocity * bonusRicochetMultiplier;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            _arrowController.haveArrow = false;
    }

}
