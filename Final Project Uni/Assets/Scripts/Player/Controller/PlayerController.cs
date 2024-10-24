using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public enum PlayerState
{
    Idle, Running, Recalling, ReverseRecalling, Stunning, Rolling, Striking
}

public class PlayerController : MonoBehaviour
{
    #region Variables

    [FoldoutGroup("Stats")] 
    public PlayerStats _stats;
    [FoldoutGroup("Stats")]
    public Health PlayerHealth;
    [FoldoutGroup("Stats/Lunge")]
    public float meleeLungeForce = 100f, lungeAngle = 60f, rangeAngleTolerance = 5f, lungeRange = 25f, LungeTime = 0.3f;


    [FoldoutGroup("Debug")]
    public PlayerState currentState;
    [FoldoutGroup("Debug")]
    [ReadOnly] public Vector2 moveInput, moveBuffer;
    [FoldoutGroup("Debug")]
    public Vector2 joyStickInput;
    [FoldoutGroup("Debug")]
    public bool isJoystickInput;
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private float currentAccel;
    [FoldoutGroup("Debug/Roll")]
    [SerializeField, ReadOnly] private bool canRoll;
    [FoldoutGroup("Debug/Roll")]
    [SerializeField, ReadOnly] private float currentRollCD;
    [FoldoutGroup("Debug/Roll")]
    [SerializeField, ReadOnly] private Vector3 RollDirect;
    [FoldoutGroup("Debug/Striking")]
    [SerializeField, ReadOnly] public float strikeMultiplier;
    [FoldoutGroup("Debug/Reverse Recall")]
    [SerializeField, ReadOnly] public float ReverseRecallMultiplier = 1;

    [FoldoutGroup("Setup")] public Button interactButton;
    [FoldoutGroup("Setup")]
    public Rigidbody PlayerRB;
    [FoldoutGroup("Setup")]
    [CanBeNull] public HitStop _hitStop;
    [FoldoutGroup("Setup")]
    public PlayerAnimController _playerAnimController;
    [FoldoutGroup("Setup")]
    public ArrowController _arrowController;
    [FoldoutGroup("Setup")]
    public UltimateJoystick JoystickPA;
    [FoldoutGroup("Setup/Stamina")] public StaminaSystem staminaSystem;

    #region Calculate

    public static PlayerController Instance;

    //Calculate
    private Vector3 calculateMove,moveDirection;
    private Vector3 cameraForward, cameraRight;
    private Vector3 RecallDirect, lungeDirection;
    private Vector3 leftRayDirection, rightRayDirection;
    private Quaternion targetRotation;
    private GameObject closestEnemy;
    private float closestDistance, elapsedTime;
    private Quaternion leftRayRotation, rightRayRotation;

    #endregion

    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (PlayerHealth == null) PlayerHealth = GetComponent<Health>();
        PlayerRB = GetComponent<Rigidbody>();

        if (Instance != this || Instance != null) Destroy(Instance);
        Instance = this;
        
        interactButton.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        JoyStickInput();
        SpeedCheck();
    }

    private void Update()
    {
        UpdateRollCDTimer();

        Move(moveInput);
        UpdateAnimState();
        RotatePlayer(moveDirection, _stats.rotationSpeed);
        RollApply();
        StrikingMoveApply(strikeMultiplier);
        ReverseRecall();
    }

    private void OnDrawGizmos()
    {
        if (PlayerRB == null) return;

        //player facing
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(PlayerRB.transform.position, PlayerRB.transform.forward * lungeRange);
        
        // Calculate the boundaries of the lunge cone
        leftRayRotation = Quaternion.AngleAxis(-(lungeAngle / 2 + rangeAngleTolerance), Vector3.up);
        rightRayRotation = Quaternion.AngleAxis(lungeAngle / 2 + rangeAngleTolerance, Vector3.up);
        leftRayDirection = leftRayRotation * PlayerRB.transform.forward;
        rightRayDirection = rightRayRotation * PlayerRB.transform.forward;

        // Draw the cone boundaries
        Gizmos.color = Color.green;
        Gizmos.DrawRay(PlayerRB.transform.position, leftRayDirection * lungeRange);
        Gizmos.DrawRay(PlayerRB.transform.position, rightRayDirection * lungeRange);
    }

    #endregion

    #region Calculate

    void UpdateRollCDTimer()
    {
        if (currentRollCD > 0)
            currentRollCD -= Time.deltaTime;
        else
            canRoll = true;
    }
    void AddRollCD(float time)
    {
        currentRollCD += time;
    }

    public async UniTaskVoid doRollingMove(Vector2 input, int staminaCost)
    {
        //prevent spam in the middle
        if (!canRoll || !staminaSystem.HasEnoughStamina(staminaCost) || 
            moveBuffer == Vector2.zero || !PlayerHealth.isAlive) return;

        //add CD
        AddRollCD(_stats.rollCD + _stats.rollTime);
        canRoll = false; //just want to save calculate so I place here, hehe

        //this lead to the Roll Apply
        currentState = PlayerState.Rolling;
        _playerAnimController.DodgeAnim();

        _arrowController.isRecalling = false;

        //consume Stamina here
        staminaSystem.Consume(staminaCost);

        //roll done ? okay cool
        await UniTask.Delay(TimeSpan.FromSeconds(_stats.rollTime));
        currentState = PlayerState.Idle;
        //Might add some event here to activate particle or anything
    }
    

    void RollApply()
    {
        if (currentState == PlayerState.Rolling)
        {
            // Take from buffer
            moveDirection = (cameraRight * moveBuffer.x + cameraForward * moveBuffer.y).normalized;

            // Mix the directions
            Vector3 mixedDirection = Vector3.Lerp(transform.forward.normalized, moveDirection, _stats.controlRollDirect).normalized;

            // Implement Roll Logic here
            RollDirect.x = mixedDirection.x;
            RollDirect.z = mixedDirection.z;

            PlayerRB.velocity = RollDirect.normalized * (_stats.rollSpeed * Time.fixedDeltaTime * 240);
        }
    }
    
    public void StrikingMoveApply(float speedMultiplier = 2)
    {
        if (currentState == PlayerState.Striking)
        {
            // Take from buffer
            moveDirection = (cameraRight * moveBuffer.x + cameraForward * moveBuffer.y).normalized;

            // Mix the directions
            Vector3 mixedDirection = Vector3.Lerp(transform.forward.normalized, moveDirection, _stats.controlRollDirect).normalized;

            // Implement Roll Logic here
            RollDirect.x = mixedDirection.x;
            RollDirect.z = mixedDirection.z;

            PlayerRB.velocity = RollDirect.normalized * (_stats.rollSpeed * speedMultiplier * Time.fixedDeltaTime * 240);
        }
    }

    void LimitSpeed(float MaxSpeed)
    {
        Mathf.Clamp(PlayerRB.velocity.magnitude, 0, MaxSpeed);
        if (PlayerRB.velocity.magnitude > MaxSpeed)
            PlayerRB.velocity = PlayerRB.velocity.normalized * MaxSpeed;
    }

    #endregion

    #region Input

    public void InputMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

        if (moveInput != Vector2.zero && moveInput != moveBuffer) moveBuffer = moveInput;
    }

    public void InputRoll(InputAction.CallbackContext ctx)
    {
        if (!PlayerHealth.isAlive) return;
        Roll();
    }

    void JoyStickInput()
    {
        joyStickInput.x = JoystickPA.GetHorizontalAxis();
        joyStickInput.y = JoystickPA.GetVerticalAxis();

        isJoystickInput = (joyStickInput != Vector2.zero);

        if (joyStickInput != Vector2.zero && joyStickInput != moveBuffer)
            moveBuffer = joyStickInput;
    }

    #endregion

    #region Animation

    void UpdateAnimState()
    {
        if (currentState == PlayerState.Stunning || currentState == PlayerState.Rolling || currentState == PlayerState.Striking) goto Skip;
        if (currentState == PlayerState.ReverseRecalling) goto ReverseRecallFlag;

        currentState = PlayerState.Idle;
        if (moveDirection != Vector3.zero) currentState = PlayerState.Running;
        _playerAnimController.UpdateRunInput(currentState == PlayerState.Running);
        if (_arrowController.isRecalling) currentState = PlayerState.Recalling;
        _playerAnimController.RecallAnim(_arrowController.isRecalling);
        
        ReverseRecallFlag:
        if (currentState == PlayerState.ReverseRecalling)
        {
            _playerAnimController.RecallAnim(true, true);
            _playerAnimController.RecallAnimTrigger();
        }

        Skip:;
    }

    public async UniTaskVoid GuardAnim()
    {
        _playerAnimController.GuardAnim(true);
        await UniTask.Delay(TimeSpan.FromSeconds(_stats.guardTime));
        _playerAnimController.GuardAnim(false);
    }

    public void MeleeAnim()
    {
        if(currentState == PlayerState.Recalling || currentState == PlayerState.ReverseRecalling || currentState == PlayerState.Stunning) return;
        if(_arrowController.ChargingInput) return;
        _playerAnimController.Slash();
    }

    [Button]
    public void MeleeLunge()
    {
        //Debug.Log("Lunge");
        // Use OverlapSphere to detect all colliders within the radius
        Collider[] hitColliders = Physics.OverlapSphere(PlayerRB.transform.position, lungeRange);

        // Variable to track the closest enemy within the field of view
        closestEnemy = null;
        closestDistance = Mathf.Infinity;
        lungeDirection = PlayerRB.transform.forward.normalized;  // Default to forward if no enemy is found

        // Loop through all colliders detected by the OverlapSphere
        foreach (Collider hitCollider in hitColliders)
        {
            GameObject hitObject = hitCollider.gameObject;

            // Guard clause: If the hit object doesn't have the tag "Enemy", skip it
            if (!hitObject.CompareTag("Enemy")) continue;

            Vector3 directionToEnemy = (hitObject.transform.position - PlayerRB.transform.position).normalized;

            // Calculate the angle between the player's forward direction and the direction to the enemy
            float angleToEnemy = Vector3.Angle(PlayerRB.transform.forward, directionToEnemy);

            // Guard clause: If the enemy is outside the cone of ±30 degrees, skip it
            if (angleToEnemy > lungeAngle / 2 + rangeAngleTolerance) continue;

            // Calculate the distance to the enemy
            float distanceToEnemy = Vector3.Distance(PlayerRB.transform.position, hitObject.transform.position);

            // Guard clause: If this enemy is farther than the closest found, skip it
            if (distanceToEnemy >= closestDistance) continue;

            // Update closest enemy and distance if this one is closer
            closestEnemy = hitObject;
            closestDistance = distanceToEnemy;
            
            // Draw a line to this enemy for debug purposes
            Debug.DrawLine(PlayerRB.transform.position, hitObject.transform.position, Color.red, 1f);
        }

        // Guard clause: If no enemy was found, just lunge forward
        if (closestEnemy != null)
            lungeDirection = (closestEnemy.transform.position - PlayerRB.transform.position).normalized;

        doMeleeLunge(LungeTime);
    }
    
    public async UniTaskVoid doMeleeLunge(float time)
    {
        elapsedTime = 0f;  // Tracks how much time has passed

        while (elapsedTime < time)
        {
            PlayerRB.AddForce(lungeDirection * meleeLungeForce, ForceMode.Acceleration);
            RotatePlayer(lungeDirection, _stats.rotationSpeed * 0.75f);
            
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            elapsedTime += Time.deltaTime;
        }

        //Debug.Log("Lunge Rotation Complete");
    }

    #endregion

    #region Movement

    public void Move(Vector2 input)
    {
        if (currentState == PlayerState.Stunning || currentState == PlayerState.Rolling ||
            currentState == PlayerState.Recalling  || currentState == PlayerState.ReverseRecalling || 
            currentState == PlayerState.Striking || !PlayerHealth.isAlive) return;

        if (isJoystickInput) input = joyStickInput;

        // Calculate camera forward direction
        cameraForward = Camera.main.transform.forward.normalized;
        cameraRight = Camera.main.transform.right.normalized;

        // Calculate the move direction based on input and camera orientation

        moveDirection = (cameraRight * input.x + cameraForward * input.y).normalized;
        moveDirection.y = 0;
        
        // Move the Rigidbody
        if (currentState != PlayerState.Rolling)
            PlayerRB.AddForce(moveDirection * (Time.deltaTime * 240 * _stats.speed), ForceMode.VelocityChange);

        LimitSpeed(_stats.maxSpeed);
    }

    void RotatePlayer(Vector3 moveDirection, float rotationSpeed)
    {
        if (moveDirection == Vector3.zero) return;
        targetRotation = Quaternion.LookRotation(moveDirection);
        // Create a new rotation with X set to 0
        targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        PlayerRB.rotation = Quaternion.Slerp(PlayerRB.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    #endregion

    #region Special Move

    public void Roll()
    {
        if (currentState == PlayerState.Rolling || moveBuffer == Vector2.zero) return;
        doRollingMove(moveBuffer, _stats.staminaRollCost);
    }
    public void Guard()
    {
        if(!PlayerHealth.isAlive) return;
        GuardAnim();
    }
    
    public void ReverseRecall()
    {
        if(currentState != PlayerState.ReverseRecalling || !_arrowController.MainArrow.isActiveAndEnabled) return;
        RecallDirect = _arrowController.MainArrow.transform.position - transform.position;
        PlayerRB.AddForce(RecallDirect.normalized * (ReverseRecallMultiplier * (_arrowController.MainArrow.recallSpeed * Time.fixedDeltaTime * 240)), ForceMode.Acceleration);
        LimitSpeed(_arrowController.MainArrow.MaxSpeed);
        
        RotatePlayer(RecallDirect, _stats.rotationSpeed/2);
    }

    #endregion

    #region Event

    [Button]
    public void Hurt(float Damage)
    {
        HurtAnim();
    }

    public void HurtAnim()
    {
        _playerAnimController.DamagedAnim();
    }

    public void ReceiveKnockback(Vector3 KnockDirect)
    {
        doReceiveKnockback(KnockDirect);
    }
    
    public async UniTaskVoid doReceiveKnockback(Vector3 KnockDirect, float StunTime = 0.15f)
    {
        //Debug.Log("Player Knockback: " + KnockDirect);
        //Implement Knockback shiet here
        KnockDirect.y = 0;
        
        PlayerRB.AddForce(KnockDirect.normalized * KnockDirect.magnitude, ForceMode.Impulse);

        currentState = PlayerState.Stunning;
        await UniTask.Delay(TimeSpan.FromSeconds(StunTime));
        currentState = PlayerState.Idle;
    }

    public void Die()
    {
        Debug.Log("Ded");
        _playerAnimController.DieAnim(true);
        PlayerHealth.isAlive = false;
    }

    public

    #endregion

    #region Debug

    void SpeedCheck()
    {
        currentAccel = PlayerRB.velocity.magnitude;
    }

    [FoldoutGroup("Debug")]
    [Button]
    public void forceChangeState(PlayerState state)
    {
        currentState = state;
    }

    #endregion
}
