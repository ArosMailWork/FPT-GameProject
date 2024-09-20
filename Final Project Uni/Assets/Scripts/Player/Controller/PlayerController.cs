using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Cysharp.Threading.Tasks;
using Enemy;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public enum PlayerState
{
    Idle,
    Running,
    Recalling,
    Stunning,
    Rolling
}

public class PlayerController : MonoBehaviour
{
    #region Variables

    [FoldoutGroup("Stats")] public bool isAlive = true;
    [FoldoutGroup("Stats")] public float speed, rotationSpeed, MaxSpeed = 20;
    [FoldoutGroup("Stats/Roll")] public float rollSpeed, rollCD, rollTime;
    [FoldoutGroup("Stats/Roll")] public int staminaRollCost;

    [FoldoutGroup("Debug")] public PlayerState currentState;
    [FoldoutGroup("Debug")] [ReadOnly] public Vector2 moveInput, moveBuffer;
    [FoldoutGroup("Debug")] [ReadOnly] [SerializeField] private int currentStamina;

    [FoldoutGroup("Debug")] [SerializeField, ReadOnly]
    private float currentAccel;

    [FoldoutGroup("Debug/Roll")] [SerializeField, ReadOnly]
    private bool canRoll;

    [FoldoutGroup("Debug/Roll")] [SerializeField, ReadOnly]
    private float currentRollCD;

    [FoldoutGroup("Debug/Roll")] [SerializeField, ReadOnly]
    private Vector3 RollDirect;
    

    [FoldoutGroup("Setup")] public Rigidbody PlayerRB;
    [FoldoutGroup("Setup")] public PlayerAnimController _playerAnimController;
    [FoldoutGroup("Setup/Stamina")] public StaminaSystem staminaSystem;
    [FoldoutGroup("Setup/Stamina")] public int staminaRegenRate = 10;
    [FoldoutGroup("Setup/Stamina")] public int staminaMax = 100;

    [FoldoutGroup("Setup/Health")] public HealthSystem healthSystem;
    [FoldoutGroup("Setup/Health")] public int healthMax = 100;


    #region Calculate

    public static PlayerController Instance;

    //Calculate
    private Vector3 calculateMove;
    private Vector3 cameraForward;
    private Vector3 cameraRight;
    public Vector3 moveDirection;


    public UltimateJoystick JoystickPA;
    public Vector3 joyStickInput;
    public bool isJoystickInput;

    #endregion

    #endregion


    #region Unity Methods

    private void Awake()
    {
        PlayerRB = GetComponent<Rigidbody>();

        if (Instance != this || Instance != null) Destroy(Instance);
        Instance = this;
        staminaSystem = new StaminaSystem(staminaMax, staminaRegenRate);
        healthSystem = new HealthSystem(healthMax);
        healthSystem.OnDeath += Die;
        // healthSystem.OnValueChange += OnHealthChanged;
    }

    // private void OnHealthChanged(object sender)
    // {
    //     throw new NotImplementedException();
    // }

    private void FixedUpdate()
    {
        SpeedCheck();
        
    }

    private void Update()
    {
        UpdateRollCDTimer();
        JoyStickInput();
        Move(moveInput);
        RollApply();
        staminaSystem.RegenerateStamina();
        currentStamina = staminaSystem.Value; // Update the field
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

    void JoyStickInput()
    {
        float horizontal = JoystickPA.GetHorizontalAxis();
        float vertical = JoystickPA.GetVerticalAxis();
        joyStickInput.x = -horizontal;
        joyStickInput.z = -vertical;
    }

    #endregion

    #region Damage

    public int BaseDamage { get; private set; } = 10;
    public bool isAttack = false;

    //Dealing damage trigger by input
    public void Attack()
    {
        //play animation, sount, etc
    }

    #region AnimationEvent

    public void ActiveAttack()
    {
        isAttack = true;
    }

    public void CompleteAttack()
    {
        isAttack = false;
    }
    #endregion

    //Receive damage from Enemy or Trap
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<EnemyController>();
            if (enemy.isAttack == true)
            {
                healthSystem.ReceiveDamage(enemy.BaseDamage);
                // _playerAnimController.GetHit();
            }
        }
        else if (other.gameObject.CompareTag("Trap"))
        {
            // healthSystem.Damage(20);
            // _playerAnimController.GetHit();
        }
    }

    public void Die(object sender)
    {
        print("Player: Die");
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
        moveDirection = isJoystickInput ? joyStickInput : (cameraRight * input.x + cameraForward * input.y).normalized;
        moveDirection.y = 0;

        // Move the Rigidbody
        if (currentState != PlayerState.Rolling)
        {
            var force = (isJoystickInput ? joyStickInput : moveDirection) * 120 * speed * Time.deltaTime;
            PlayerRB.AddForce(force, ForceMode.VelocityChange);
        }

        RotatePlayer(moveDirection);

        LimitSpeed();
    }

    //do Roll, call by input
    public void Roll()
    {
        if (!staminaSystem.HasEnoughStamina(staminaRollCost))
        {
            //maybe blink stamina slider or something
            return;
        }
        if (currentState == PlayerState.Rolling || moveBuffer == Vector2.zero) return;
        staminaSystem.Consume(staminaRollCost);
        // print($"moveBuffer: {moveBuffer}");
        // print($"joyStickInput: {joyStickInput}");
        doRollingMove(isJoystickInput ? joyStickInput : moveBuffer);
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
    // public void ReceiveKnockback(DataPack dp)
    // {
    //     Debug.Log("Player: KNOCK dp test");
    // }

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
        print("Rolling 2");

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

    public void Shoot()
    {
    }

    public void MeleeAttack()
    {
    }
}