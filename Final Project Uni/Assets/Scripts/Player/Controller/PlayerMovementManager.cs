using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

    public class PlayerMovementManager: MonoBehaviour
    {
        public PlayerController pController;
        public Rigidbody PlayerRB;
        
        [FoldoutGroup("Stats")]
        public bool blockInput;
        [FoldoutGroup("Stats")]
        public Health PlayerHealth;
        [FoldoutGroup("Stats/Lunge")]
        public float meleeLungeForce = 100f, lungeAngle = 60f, rangeAngleTolerance = 5f, lungeRange = 25f, LungeTime = 0.3f;
        
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
        
        [FoldoutGroup("Setup")]
        public UltimateJoystick JoystickPA;
        [FoldoutGroup("Setup/Stamina")] public StaminaSystem staminaSystem;
        
        [FoldoutGroup("Debug")]
        [ReadOnly] public Vector2 moveInput = Vector2.zero, moveBuffer;
        [FoldoutGroup("Debug")]
        public Vector2 joyStickInput;
        [FoldoutGroup("Debug")]
        public bool isJoystickInput;
        
        [FoldoutGroup("Setup/Save")]
        public PlayerStats _stats;
        
        private Vector3 rollDir;
        private Vector3 calculateMove, moveDirection;
        private Vector3 cameraForward, cameraRight;
        private Vector3 RecallDirect, lungeDirection;
        private Quaternion targetRotation, leftRayRotation, rightRayRotation;
        private Vector3 leftRayDirection, rightRayDirection;
        
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
            rollDir = transform.forward.normalized;
            pController.currentState = PlayerState.Rolling;
            pController._playerAnimController.DodgeAnim();

            pController._arrowController.isRecalling = false;

            //consume Stamina here
            staminaSystem.Consume(staminaCost);

            //roll done ? okay cool
            await UniTask.Delay(TimeSpan.FromSeconds(_stats.rollTime));
            pController.currentState = PlayerState.Idle;
            //Might add some event here to activate particle or anything
        }


        void RollApply()
        {
            if (pController.currentState == PlayerState.Rolling)
            {
                // Take from buffer
                moveDirection = (cameraRight * moveBuffer.x + cameraForward * moveBuffer.y).normalized;

                // Mix the directions
                Vector3 ControlRoll = Vector3.Lerp(transform.forward.normalized, moveDirection, 0.2f).normalized;
                Vector3 mixedDirection = Vector3.Lerp(rollDir, ControlRoll, _stats.controlRollDirect).normalized;

                // Implement Roll Logic here
                RollDirect.x = mixedDirection.x;
                RollDirect.z = mixedDirection.z;

                PlayerRB.velocity = RollDirect.normalized * (_stats.rollSpeed * Time.fixedDeltaTime * 240);
            }
        }

        public void StrikingMoveApply(float speedMultiplier = 2)
        {
            if (pController.currentState == PlayerState.Striking)
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
        
        #region Movement

        public void Move(Vector2 input = default)
        {
            if (pController.currentState == PlayerState.Stunning || pController.currentState == PlayerState.Rolling ||
                pController.currentState == PlayerState.Recalling || pController.currentState == PlayerState.ReverseRecalling ||
                pController.currentState == PlayerState.Striking || !PlayerHealth.isAlive) return;

            if (isJoystickInput) input = joyStickInput;

            // Calculate camera forward direction
            cameraForward = Camera.main.transform.forward.normalized;
            cameraRight = Camera.main.transform.right.normalized;

            // Calculate the move direction based on input and camera orientation

            moveDirection = (cameraRight * input.x + cameraForward * input.y).normalized;
            moveDirection.y = 0;

            // Move the Rigidbody
            if (pController.currentState != PlayerState.Rolling)
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
            if (blockInput) return;
            if (!PlayerHealth.isAlive) return;
            if (pController.currentState == PlayerState.Rolling || moveBuffer == Vector2.zero) return;
            doRollingMove(moveBuffer, _stats.staminaRollCost);
        }

        public void ReverseRecall()
        {
            if (pController.currentState != PlayerState.ReverseRecalling || !pController._arrowController.MainArrow.isActiveAndEnabled) return;
            RecallDirect = pController._arrowController.MainArrow.transform.position - transform.position;
            PlayerRB.AddForce(RecallDirect.normalized * (ReverseRecallMultiplier * (pController._arrowController.MainArrow.recallSpeed * Time.fixedDeltaTime * 240)), ForceMode.Acceleration);
            LimitSpeed(pController._arrowController.MainArrow.MaxSpeed);

            RotatePlayer(RecallDirect, _stats.rotationSpeed / 2);
        }

        #endregion
        
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
    }
