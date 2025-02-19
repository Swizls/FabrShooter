using FabrShooter.Input;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FabrShooter.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour, IPlayerInitializableComponent
    {
        private const float GRAVITY = -9.81f;
        private const float DISTANCE_TO_DETECT_GROUND = 1.2f;
        private const float DISTANCE_TO_DETECT_SURFACE_FOR_WALL_JUMP = 2f;

        [SerializeField] private PlayerConfigSO _config;
        [SerializeField] private Camera _camera;

        private CharacterController _characterController;
        private PlayerInputActions _playerInputActions;

        private Vector3 _movementDirection;
        private Vector3 _velocity;

        private float _stamina;

        public Action Jumped;

        public Action StartedStaminaConsumption;
        public Action EndedStaminaConsumtion;

        public float Stamina => _stamina;
        public PlayerConfigSO Config => _config;
        public Vector3 MovementDirection => _movementDirection;

        public CharacterController CharacterController => _characterController;

        public bool IsRunning
        {
            get { return _playerInputActions.Player.Sprint.ReadValue<float>() > 0 && IsMoving; }
        }

        public bool IsAbleToRun
        {
            get { return _stamina > 0; }
        }

        public bool IsMoving
        {
            get { return _movementDirection.x != 0 || _movementDirection.z != 0; }
        }

        public bool IsFlying
        {
            get
            {
                return !IsGroundend;
            }
        }

        public bool IsGroundend
        {
            get
            {
                return Physics.Raycast(transform.position, Vector3.down, DISTANCE_TO_DETECT_GROUND, LayerMask.GetMask("Default"));
            }
        }

        #region MONO
        public void Initialize()
        {
            _characterController = GetComponent<CharacterController>();
            _playerInputActions = new PlayerInputActions();

            _playerInputActions.Player.Enable();
            _playerInputActions.Player.Jump.performed += Jump;
            _stamina = _config.MaxStamina;
        }

        private void OnEnable()
        {
            if (_playerInputActions == null)
                return;

            _playerInputActions.Player.Jump.performed += Jump;
            _playerInputActions.Player.Enable();
        }

        private void OnDisable()
        {
            if (_playerInputActions == null)
                return;

            _playerInputActions.Player.Jump.performed -= Jump;
            _playerInputActions.Player.Disable();
        }

        private void Update()
        {
            ListenMovementInput();
        }

        private void FixedUpdate()
        {
            Move();
            ApplyGravity();
        }

        private void ApplyGravity()
        {
            if (IsFlying == false)
            {
                _velocity.y = 0;
                return;
            }

            _velocity.y += GRAVITY * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, _movementDirection);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _velocity);
        }
        #endregion

        private void Move()
        {
            if (IsRunning && IsAbleToRun)
            {
                StopAllCoroutines();
                StartCoroutine(ConsumeStamina());
            }

            float speed = IsRunning && IsAbleToRun ? _config.WalkingSpeed * _config.SprintingMultiplier : _config.WalkingSpeed;

            if (IsFlying)
            {
                Vector3 forwardComponent = Vector3.Project(_movementDirection, _velocity.normalized);
                Vector3 sideComponent = _movementDirection - forwardComponent;

                forwardComponent *= _config.JumpInertia;
                _movementDirection = forwardComponent + sideComponent;
            }

            _velocity.x = Mathf.Lerp(_velocity.x, _movementDirection.x * speed, _config.MovementInertia);
            _velocity.z = Mathf.Lerp(_velocity.z, _movementDirection.z * speed, _config.MovementInertia);

            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (IsFlying && CanDoWallJump() == false)
                return;

            Vector3 jumpDirection = Vector3.up;

            if(!IsGroundend)
            {
                if (IsSurfaceOnGivenDirection(-transform.right))
                    jumpDirection += (transform.right + transform.forward) * _config.WallJumpForce;
                else if (IsSurfaceOnGivenDirection(transform.right))
                    jumpDirection += (-transform.right + transform.forward) * _config.WallJumpForce;
            }

            _velocity = jumpDirection * Mathf.Sqrt(_config.JumpForce * -2f * GRAVITY);

            _characterController.Move(_velocity * Time.deltaTime);
            Jumped?.Invoke();

            bool CanDoWallJump()
            {
                return IsSurfaceOnGivenDirection(transform.right) || IsSurfaceOnGivenDirection(-transform.right);
            }

            bool IsSurfaceOnGivenDirection(Vector3 direction)
            {
                return Physics.Raycast(transform.position, direction, DISTANCE_TO_DETECT_SURFACE_FOR_WALL_JUMP, LayerMask.GetMask("Default"));
            }
        }

        private IEnumerator ConsumeStamina()
        {
            StartedStaminaConsumption?.Invoke();

            while (IsRunning && IsAbleToRun)
            {
                _stamina -= _config.StaminaConsumptionSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            StartCoroutine(RestoreStamina());
        }

        private IEnumerator RestoreStamina()
        {
            while (_stamina < _config.MaxStamina)
            {
                _stamina += _config.StaminaRestoreSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            EndedStaminaConsumtion?.Invoke();
        }

        private void ListenMovementInput()
        {
            Vector2 playerInput = _playerInputActions.Player.Move.ReadValue<Vector2>();

            Vector3 forward = _camera.transform.TransformDirection(Vector3.forward);
            Vector3 right = _camera.transform.TransformDirection(Vector3.right);

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            _movementDirection = forward * playerInput.y + right * playerInput.x;
        }
    }
}