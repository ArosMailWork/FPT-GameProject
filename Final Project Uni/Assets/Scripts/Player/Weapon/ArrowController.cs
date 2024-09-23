using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowController : MonoBehaviour
{
    private PlayerController _playerController;
    
    [FoldoutGroup("Stats")]
    public AnimationCurve forceCurve;
    
    [FoldoutGroup("Stats")]
    public float ShootForce, currentChargedTime, chargedTime = 2f;

    [FoldoutGroup("Debug")]
    [SerializeField] List<Arrow> arrowsList;
    [FoldoutGroup("Debug/States")]
    [ReadOnly] public bool ChargingInput;
    [FoldoutGroup("Debug/States")]
    public bool IsCharging, FullyCharged, isRecalling, haveArrow;

    [FoldoutGroup("Debug/Setup")] public GameObject ArrowPrefab;
    
    
    //Calculate
    public static ArrowController Instance;

    #region Unity Event

    private void Awake()
    {
        if (Instance != this || Instance != null) Destroy(Instance);
        Instance = this;
    }
    private void Start()
    {
        _playerController = PlayerController.Instance;
    }

    private void Update()
    {
        UpdateCharging();
    }

    #endregion

    #region Input

    public void ChargeShoot(InputAction.CallbackContext ctx)
    {
        //have arrow and alive ? cool
        if (!haveArrow || !_playerController.isAlive) return;
        
        ChargingInput = ctx.performed;
    }
    
    public void Recall(InputAction.CallbackContext ctx)
    {
        if(haveArrow || !_playerController.isAlive) return;
        
        isRecalling = ctx.performed;
        StartRecall(isRecalling);
    }
    
    public void ChargeShoot()
    {
        //have arrow and alive ? cool
        if (!haveArrow || !_playerController.isAlive) return;
        ChargingInput = true;
        IsCharging = true;
    }
    
    public void OffRecall()
    {
        StartRecall(false);
    }

    #endregion

    #region Shoot

    void UpdateCharging()
    {
        //check to charge, if release charge input 
        
        //fix this part allow hold, also recommend to add a cancel boolean to check
        //If it true, even release wont shoot - Duck 
        if (ChargingInput && IsCharging)
        {
            if (currentChargedTime <= chargedTime)
            {
                currentChargedTime += Time.deltaTime;
            }
            else
            {
                IsCharging = false;
                //Shoot( null);
            }
        }
    }
    void StartCharge()
    {
        IsCharging = true;
        if(currentChargedTime <= chargedTime) currentChargedTime += Time.deltaTime;
        
        //might have more
    }
    
    [Button]
    public void Shoot()
    {
        if (!haveArrow) return;
        //beacuse button up
        ChargingInput = false;
        IsCharging = false;
        haveArrow = false;

        foreach (var arrow in arrowsList)
        {
            float calForce = forceCurve.Evaluate(currentChargedTime / chargedTime) * ShootForce;
            Vector3 ShootDir = _playerController.transform.forward;
            ShootDir.y = 0;
            
            //Test (pooling replace)
            arrow.gameObject.SetActive(true);
            arrow.transform.position = _playerController.transform.position;
            
            arrow.Shoot(ShootDir.normalized);
            arrow.currentArrowState = ArrowState.Shooting;
        }

        currentChargedTime = 0;
    }
    
    #endregion

    #region Recall

    [Button]
    public void StartRecall(bool isRecalling)
    {
        if(!_playerController.isAlive || _playerController.currentState == PlayerState.Stunning) return;

        if (isRecalling) _playerController.currentState = PlayerState.Recalling;
        else _playerController.currentState = PlayerState.Idle;
        
        foreach (var arrow in arrowsList)
        {
            if(arrow == null || arrow.currentArrowState == ArrowState.Shooting) return;

            if (isRecalling)
            {
                //cant call while shooting
                if(arrow.currentArrowState != ArrowState.Shooting) 
                    arrow.currentArrowState = ArrowState.Recalling;
            }
            else
                arrow.currentArrowState = ArrowState.Idle;
        }
    }
    #endregion
}
