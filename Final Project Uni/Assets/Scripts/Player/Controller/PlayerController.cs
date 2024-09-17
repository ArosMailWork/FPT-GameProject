using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Cysharp.Threading.Tasks;
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
    public bool isAlive = true;
    [FoldoutGroup("Stats")]
    public float speed, rotationSpeed, MaxSpeed = 20;
    [FoldoutGroup("Stats/Roll")]
    public float rollSpeed, rollCD, rollTime;


    [FoldoutGroup("Debug")]
    public PlayerState currentState;
    [FoldoutGroup("Debug")]
    [ReadOnly] public Vector2 moveInput, moveBuffer;
    [FoldoutGroup("Debug")]
    [SerializeField, ReadOnly] private float currentAccel;
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

    #region Calculate

    public static PlayerController Instance;

    //Calculate
    private Vector3 calculateMove;
    private Vector3 cameraForward;
    private Vector3 cameraRight;
    private Vector3 moveDirection;
    //private StaminaSystem staminaSystem;

    #endregion

    #endregion



    #region Unity Methods
    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody>();

        if (Instance != this || Instance != null) Destroy(Instance);
        Instance = this;
        //staminaSystem = new StaminaSystem(100, 10);
    }

    private void FixedUpdate()
    {
        SpeedCheck();
        //staminaSystem.RegenerateStamina();
    }

    private void Update()
    {
        UpdateRollCDTimer();

        Move(moveInput);
        RollApply();
    }

    private void OnDrawGizmos()
    {

    }

    #endregion

    #region Input

    public void InputMove(InputAction.CallbackContext ctx)
    {
        if (!isAlive) return;
        moveInput = ctx.ReadValue<Vector2>();

        if (moveInput != Vector2.zero && moveInput != moveBuffer) moveBuffer = moveInput;
    }

    public void InputRoll(InputAction.CallbackContext ctx)
    {
        if (!isAlive) return;
        Roll();
    }

    #endregion

    #region Movement

    public void Move(Vector2 input)
    {
        if (currentState == PlayerState.Stunning ||
            currentState == PlayerState.Rolling ||
            currentState == PlayerState.Recalling) return;

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
            //PlayerRB.AddForce(moveDirection * speed, ForceMode.Acceleration);
            PlayerRB.AddForce(moveDirection * (Time.deltaTime * 240 * speed), ForceMode.VelocityChange);

        RotatePlayer(moveDirection);
        
        _playerAnimController.UpdateRunInput(moveInput); //test

        LimitSpeed();
    }

    //do Roll, call by input
    public void Roll()
    {
        if (currentState == PlayerState.Rolling || moveBuffer == Vector2.zero) return;
        //if (!staminaSystem.CheckEnoughStamina) return;
        doRollingMove(moveBuffer);
    }

    void RotatePlayer(Vector3 moveDirection)
    {
        if (moveDirection == Vector3.zero) return;
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        PlayerRB.rotation = Quaternion.Slerp(PlayerRB.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    #endregion

    #region Stats

    public void Hurt()
    {
        Debug.Log("Player: ouch");
    }

    //TEST
    public void ReceiveKnockback(DataPack dp)
    {
        Debug.Log("Player: KNOCK dp test");
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
        if (!canRoll) return;

        //add CD
        AddRollCD(rollCD + rollTime);
        canRoll = false; //just want to save calculate so I place here, hehe

        //this lead to the Roll Apply do non-stop
        currentState = PlayerState.Rolling;
        Debug.DrawRay(PlayerRB.transform.position, moveDirection, Color.blue, 0.2f);
        //consume Stamina here

        //roll done ? okay cool
        await UniTask.Delay(TimeSpan.FromSeconds(rollTime));
        currentState = PlayerState.Idle;
        //Might add some event here to activate particle or anything
    }

    void RollApply()
    {
        if (currentState == PlayerState.Rolling)
        {
            //implement Roll Logic here
            RollDirect.x = moveDirection.x;
            RollDirect.z = moveDirection.z;
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

    #region Debug

    void SpeedCheck()
    {
        currentAccel = PlayerRB.velocity.magnitude;
    }

    #endregion
}
