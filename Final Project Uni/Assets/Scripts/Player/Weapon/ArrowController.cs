using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowController : MonoBehaviour
{
    #region Variables

    private PlayerController _playerController;
    public PlayerAnimController _playerAnimController;

    [FoldoutGroup("Stats")]
    public AnimationCurve forceCurve;

    [FoldoutGroup("Stats")]
    public float ShootForce, currentChargedTime, chargedTime = 2f;

    [FoldoutGroup("Debug")]
    [SerializeField] List<Arrow> arrowsList;
    [FoldoutGroup("Debug/States")]
    [ReadOnly] public bool ChargingInput;
    [FoldoutGroup("Debug/States")]
    public bool FullyCharged, isRecalling, RecallBuffer, arrowRecoverFlag, haveArrow;
    [FoldoutGroup("Debug/States")]
    public bool ShootButtonPressing;

    [FoldoutGroup("Debug/Setup")] public GameObject ArrowPrefab;
    [FoldoutGroup("Debug/Setup")] public ParticleManager prefabParticleManager;
    [FoldoutGroup("Debug/Setup")] public Arrow MainArrow;
    [FoldoutGroup("Debug/Buff")] public bool IsSplitShot = false;

    public static ArrowController Instance;

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
        _playerAnimController = _playerController._playerAnimController;

        haveArrow = true;
        _playerAnimController.UpdateHaveArrow(true);
    }

    private void Update()
    {
        UpdateCharging();

        if (RecallBuffer)
        {
            RecallArrow(MainArrow);
            
            if(!IsSplitShot) return;
            foreach (var arrow in arrowsList)
            {
                if (!arrow.IsMainArrow) RecallArrow(arrow);
            }
        }

    }

    #endregion

    #region Input

    public void ChargeShoot(InputAction.CallbackContext ctx)
    {
        ChargeShoot(ctx.performed);
    }
    public void Recall(InputAction.CallbackContext ctx)
    {
        Recall(ctx.performed);
    }

    //Mobile Input
    public void ChargeShoot(bool charge)
    {
        //have arrow and alive ? cool
        if (!haveArrow || !_playerController.PlayerHealth.isAlive) return;
        ChargingInput = charge;
        
        _playerAnimController.Draw(true, true);
    }
    public void Recall(bool recall)
    {
        ShootButtonPressing = recall;
        if (haveArrow || !_playerController.PlayerHealth.isAlive) return;
        isRecalling = recall;
        StartRecall(recall);
    }

    #endregion

    #region Shoot

    void UpdateCharging()
    {
        if (ChargingInput)
        {
            if (currentChargedTime <= chargedTime)
            {
                currentChargedTime += Time.deltaTime;
                //Debug.Log(currentChargedTime);
            }
        }
    }
    void ShootArrow(Arrow arrow)
    {
        float calForce = forceCurve.Evaluate(currentChargedTime / chargedTime) * ShootForce;

        // Calculate the rotation around the Y-axis based on the offset in degrees
        Quaternion rotationOffset = Quaternion.Euler(0, arrow.offset, 0);

        // Apply the rotation offset to the player's forward direction (ignore Y-axis for shooting)
        Vector3 shootDir = rotationOffset * _playerController.transform.forward;
        shootDir.y = 0; // Ensure no vertical component in the shooting direction

        // Activate the arrow game object
        arrow.gameObject.SetActive(true);
        arrow.transform.position = _playerController.transform.position;

        // Set the rotation so the Z-axis points in the shoot direction
        arrow.transform.rotation = Quaternion.LookRotation(shootDir.normalized);

        // Shoot the arrow with the calculated force
        arrow.Shoot(shootDir.normalized * calForce);
        arrow.currentArrowState = ArrowState.Shooting;
    }


    [Button]
    public void Shoot()
    {
        if (!haveArrow || arrowRecoverFlag)
        {
            arrowRecoverFlag = false;
            return;
        }

        //because button up
        ChargingInput = false;
        haveArrow = false;
        _playerAnimController.UpdateHaveArrow(haveArrow);
        ShootArrow(MainArrow);
        if (IsSplitShot)
        {
            foreach (var arrow in arrowsList)
            {
                if(!arrow.IsMainArrow) ShootArrow(arrow);
            }
        }

        //wear
        _playerAnimController.Draw(false, true);
        currentChargedTime = 0;
    }

    #endregion

    #region Recall
    public async UniTaskVoid HideAllMirageArrow(float time = 0)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        foreach (var arrow in arrowsList)
        {
            if(!arrow.IsMainArrow) arrow.HideArrow();
        }
    }
    void RecallArrow(Arrow arrow)
    {
        if (arrow == null) return;
        if (arrow.currentArrowState == ArrowState.Shooting)
        {
            RecallBuffer = true;
            return;
        }

        if (isRecalling)
        {
            RecallBuffer = false;
            
            
            //cant call while shooting
            if (arrow.currentArrowState != ArrowState.Shooting)
            {
                arrow.StartRecallEvent.Invoke();
                
                arrow.currentArrowState = ArrowState.Recalling;
                if (arrow.IsMainArrow && arrow.currentArrowState == ArrowState.Recalling)
                {
                    prefabParticleManager.PlayAssignedParticle("RecallingMainArrowVFX");
                    prefabParticleManager.PlayAssignedParticle("RecallingVFX");
                }
            }
        }
        else
        {
            arrow.StopRecallEvent.Invoke();
            arrow.currentArrowState = ArrowState.Idle;
        }
    }
    [Button]
    public void StartRecall(bool recallBool)
    {
        isRecalling = recallBool;
        if (!_playerController.PlayerHealth.isAlive || _playerController.currentState == PlayerState.Stunning || haveArrow) return;

        if (recallBool)
        {
            _playerController.currentState = PlayerState.Recalling;
            _playerAnimController.RecallAnimTrigger();
        }
        else 
            _playerController.currentState = PlayerState.Idle;
        
        RecallArrow(MainArrow);
        if (!IsSplitShot) return;
        
        foreach (var arrow in arrowsList)
        {
            RecallArrow(arrow);
        }

    }
    #endregion

    #region Remote Recover

    public void RemoteRecover()
    {
        Debug.Log("Remote Recover");
        prefabParticleManager.PlayAssignedParticle("RecallingMainArrowVFX");
        foreach (var arrow in arrowsList)
        {
            arrow.HideArrow();
        }

        haveArrow = true;
        isRecalling = false;
    }

    #endregion
}
