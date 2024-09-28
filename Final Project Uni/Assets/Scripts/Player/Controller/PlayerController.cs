using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Cysharp.Threading.Tasks;
using OpenCover.Framework.Model;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public enum PlayerState
{
    Idle, Running, Recalling, Stunning, Rolling 
}

public class PlayerController : MonoBehaviour
{
    #region Variables

    [FoldoutGroup("Stats")] 
    public Health PlayerHealth;
    [FoldoutGroup("Stats")]
    public float speed, rotationSpeed, MaxSpeed = 20;
    [FoldoutGroup("Stats/Roll")]
    public float rollSpeed, rollCD, rollTime;
    [FoldoutGroup("Setup/Stamina")] 
    public int staminaRollCost;
    [FoldoutGroup("Setup/Guard")] 
    public float guardTime;


    [FoldoutGroup("Debug")]
    public PlayerState currentState;
    [FoldoutGroup("Debug")]
    [ReadOnly] public Vector2 moveInput, moveBuffer;
    [FoldoutGroup("Debug")]
    public Vector2 joyStickInput;
    [FoldoutGroup("Debug")]
    public bool isJoystickInput;
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private float currentAccel, controlRollDirect = 0.2f;
    [FoldoutGroup("Debug/Roll")]
    [SerializeField, ReadOnly] private bool canRoll;
    [FoldoutGroup("Debug/Roll")]
    [SerializeField, ReadOnly] private float currentRollCD;
    [FoldoutGroup("Debug/Roll")]
    [SerializeField, ReadOnly] private Vector3 RollDirect;

    [FoldoutGroup("Setup")]
    public Rigidbody PlayerRB;
    [FoldoutGroup("Setup")]
    public PlayerAnimController _playerAnimController;
    [FoldoutGroup("Setup")]
    public ArrowController _ArrowController;
    [FoldoutGroup("Setup")]
    public UltimateJoystick JoystickPA;
    [FoldoutGroup("Setup/Stamina")] public StaminaSystem staminaSystem;

    #region Calculate

    public static PlayerController Instance;

    //Calculate
    private Vector3 calculateMove;
    private Vector3 cameraForward;
    private Vector3 cameraRight;
    private Vector3 moveDirection;
    private Quaternion targetRotation;

    #endregion

    #endregion
    
    #region Unity Methods
    private void Awake()
    {
        if (PlayerHealth == null) PlayerHealth = GetComponent<Health>();
        PlayerRB = GetComponent<Rigidbody>();

        if (Instance != this || Instance != null) Destroy(Instance);
        Instance = this;
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
        RotatePlayer(moveDirection);
        RollApply();
    }

    private void OnDrawGizmos()
    {

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

    public async UniTaskVoid doRollingMove(Vector2 input)
    {
        //prevent spam in the middle
        if (!canRoll || !staminaSystem.HasEnoughStamina(staminaRollCost) || moveBuffer == Vector2.zero) return;

        //add CD
        AddRollCD(rollCD + rollTime);
        canRoll = false; //just want to save calculate so I place here, hehe

        //this lead to the Roll Apply do non-stop
        
        currentState = PlayerState.Rolling;
        _playerAnimController.DodgeAnim();
        //consume Stamina here
        staminaSystem.Consume(staminaRollCost);

        //roll done ? okay cool
        await UniTask.Delay(TimeSpan.FromSeconds(rollTime));
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
            Vector3 mixedDirection = Vector3.Lerp(transform.forward.normalized, moveDirection, controlRollDirect).normalized;

            // Implement Roll Logic here
            RollDirect.x = mixedDirection.x;
            RollDirect.z = mixedDirection.z;

            PlayerRB.velocity = RollDirect.normalized * (rollSpeed * Time.fixedDeltaTime * 240);
        }
    }

    void LimitSpeed()
    {
        Mathf.Clamp(PlayerRB.velocity.magnitude, 0, MaxSpeed);
        if (PlayerRB.velocity.magnitude > MaxSpeed)
            PlayerRB.velocity = PlayerRB.velocity.normalized * MaxSpeed;
    }

    #endregion

    #region Input

    public void InputMove(InputAction.CallbackContext ctx)
    {
        if (!PlayerHealth.isAlive) return;
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
        if(currentState == PlayerState.Stunning || currentState == PlayerState.Rolling) goto Skip;
        
        currentState = PlayerState.Idle;
        if(moveDirection != Vector3.zero) currentState = PlayerState.Running;
        if (_ArrowController.isRecalling) currentState = PlayerState.Recalling;
        
        _playerAnimController.UpdateRunInput(currentState == PlayerState.Running);

        Skip:;
    }
    
    public async UniTaskVoid GuardAnim()
    {
        _playerAnimController.GuardAnim(true);
        await UniTask.Delay(TimeSpan.FromSeconds(guardTime));
        _playerAnimController.GuardAnim(false);
    }

    #endregion

    #region Movement

    public void Move(Vector2 input)
    {
        if (currentState == PlayerState.Stunning ||
            currentState == PlayerState.Rolling ||
            currentState == PlayerState.Recalling) return;

        if (isJoystickInput) input = joyStickInput;

        // Calculate camera forward direction
        cameraForward = Camera.main.transform.forward.normalized;
        cameraRight = Camera.main.transform.right.normalized;

        // Calculate the move direction based on input and camera orientation
        
        moveDirection = (cameraRight * input.x + cameraForward * input.y).normalized;
        moveDirection.y = 0;

        // Debugging: Draw a ray in the direction of movement
        //Debug.DrawRay(PlayerRB.transform.position, moveDirection, Color.blue, 0.2f);

        // Move the Rigidbody
        if (currentState != PlayerState.Rolling)
            PlayerRB.AddForce(moveDirection * (Time.deltaTime * 240 * speed), ForceMode.VelocityChange);

        LimitSpeed();
    }

    void RotatePlayer(Vector3 moveDirection)
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
        doRollingMove(moveBuffer);
    }
    
    public void Guard()
    {
        GuardAnim();
    }

    #endregion

    #region Event

    [Button]
    public void Hurt(Vector3 KnockDirect, float Damage)
    {
        _playerAnimController.DamagedAnim();
        ReceiveKnockback(KnockDirect);
    }
    
    public void ReceiveKnockback(Vector3 KnockDirect)
    {
        Debug.Log("Player: ouch");
        //Implement Knockback shiet here
    }

    public void Die()
    {
        Debug.Log("Ded");
        _playerAnimController.DieAnim(true);
    }
    
    public

    #endregion
    
    #region Debug

    void SpeedCheck()
    {
        currentAccel = PlayerRB.velocity.magnitude;
    }

    #endregion
}
