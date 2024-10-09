using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowController : MonoBehaviour
{
    #region Variables

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
    public bool IsCharging, FullyCharged, isRecalling, isCanceling, haveArrow;

    [FoldoutGroup("Debug/Setup")] public GameObject ArrowPrefab;
    [FoldoutGroup("Debug/Setup")]
    public Arrow MainArrow;

    public static ArrowController Instance;
    [FoldoutGroup("Debug/Buff")] public bool IsSplitShot = false;

    #endregion

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
        if (!haveArrow || !_playerController.PlayerHealth.isAlive) return;
        //if (!haveArrow) return;
        ChargingInput = ctx.performed;
    }
    public void Recall(InputAction.CallbackContext ctx)
    {
        //if (haveArrow || !_playerController.isAlive) return;
        if (haveArrow) return;
        isRecalling = ctx.performed;
        StartRecall(isRecalling);
    }

    //Mobile Input
    public void ChargeShoot(bool charge)
    {
        //have arrow and alive ? cool
        if (!haveArrow || !_playerController.PlayerHealth.isAlive) return;
        //if (haveArrow) return;
        ChargingInput = charge;
        Debug.Log(ChargingInput);
    }
    public void Recall(bool recall)
    {
        if (haveArrow || !_playerController.PlayerHealth.isAlive) return;
        //if (haveArrow) return;
        isRecalling = recall;
        StartRecall(isRecalling);
    }

    #endregion

    #region Shoot

    void UpdateCharging()
    {
        //if holding charge 
        // if (ChargingInput && IsCharging && !isCanceling)
        // {
        //     if (currentChargedTime <= chargedTime)
        //     {
        //         currentChargedTime += Time.deltaTime;
        //         Debug.Log(currentChargedTime);
        //     }
        // }
        //if holding charge 
        if (ChargingInput)
        {
            if (currentChargedTime <= chargedTime)
            {
                currentChargedTime += Time.deltaTime;
                Debug.Log(currentChargedTime);
            }
        }
    }
    void ShootArrow(Arrow arrow)
    {

        float calForce = forceCurve.Evaluate(currentChargedTime / chargedTime) * ShootForce;
        Vector3 ShootDir = _playerController.transform.forward + new Vector3(arrow.offset, arrow.offset, arrow.offset);
        ShootDir.y = 0;
        //Test (pooling replace)
        arrow.gameObject.SetActive(true);
        arrow.transform.position = _playerController.transform.position;
        arrow.Shoot(ShootDir.normalized * calForce);
        //arrow.Shoot(ShootDir.normalized);
        arrow.currentArrowState = ArrowState.Shooting;
    }
    [Button]
    public void Shoot()
    {
        if (!haveArrow) return;
        //beacuse button up
        ChargingInput = false;
        IsCharging = false;
        haveArrow = false;
        ShootArrow(MainArrow);
        if (IsSplitShot)
        {
            foreach (var arrow in arrowsList)
            {
                ShootArrow(arrow);
            }
        }


        currentChargedTime = 0;
    }

    #endregion

    #region Recall
    public void HideAllArrow()
    {
        foreach (var arrow in arrowsList)
        {
            arrow.HideArrow();
        }
    }
    void RecallArrow(Arrow arrow)
    {
        if (arrow == null || arrow.currentArrowState == ArrowState.Shooting) return;

        if (isRecalling)
        {
            //cant call while shooting
            if (arrow.currentArrowState != ArrowState.Shooting)
                arrow.currentArrowState = ArrowState.Recalling;
        }
        else
            arrow.currentArrowState = ArrowState.Idle;
    }
    [Button]
    public void StartRecall(bool isRecalling)
    {
        if (!_playerController.PlayerHealth.isAlive || _playerController.currentState == PlayerState.Stunning || haveArrow) return;
        //if (_playerController.currentState == PlayerState.Stunning || haveArrow) return;
        if (isRecalling) _playerController.currentState = PlayerState.Recalling;
        else _playerController.currentState = PlayerState.Idle;
        RecallArrow(MainArrow);
        if (IsSplitShot)
        {
            foreach (var arrow in arrowsList)
            {
                RecallArrow(arrow);
            }
        }

    }
    #endregion
}
