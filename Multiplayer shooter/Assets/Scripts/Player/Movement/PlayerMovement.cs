using FabrShooter.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FabrShooter.Player.Movement
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
        private Mover _currentMover;

        public Action Jumped;
        public Action StartedStaminaConsumption;
        public Action EndedStaminaConsumtion;

        [HideInInspector] public Vector3 Velocity;

        public PlayerConfigSO Config => _config;
        public Vector2 InputDirection => _playerInputActions.Player.Move.ReadValue<Vector2>();
        public CharacterController CharacterController => _characterController;
        public Mover CurrentMover => _currentMover;
        public bool IsSliding { get; private set; }

        public bool IsRunning
        {
            get { return _playerInputActions.Player.Sprint.ReadValue<float>() > 0 && IsMoving; }
        }

        public bool IsMoving
        {
            get { return CharacterController.velocity.magnitude > 0; }
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
        public void InitializeLocalPlayer()
        {
            _characterController = GetComponent<CharacterController>();
            _playerInputActions = new PlayerInputActions();
            _currentMover = new WalkMover(this, _playerInputActions, _camera);

            _playerInputActions.Player.Enable();
            _playerInputActions.Player.Jump.performed += Jump;
        }

        public void InitializeClientPlayer()
        {
            _characterController.enabled = false;
            Destroy(this);
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
            ListenSlideInput();
        }

        private void FixedUpdate()
        {
            _currentMover.ListenMovementInput();
            _currentMover.Move();
            ApplyGravity();
        }


        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Velocity);
        }
        #endregion

        private void ApplyGravity()
        {
            if (!IsFlying)
            {
                Velocity.y = 0;
                return;
            }

            Velocity.y += GRAVITY * Time.deltaTime;
            _characterController.Move(Velocity * Time.deltaTime);
        }

        private void ChangeMover<T> (T value) where T : Mover
        {
            //_currentMover = new T();
        }

        private void ListenSlideInput()
        {
            if (IsRunning == false || _playerInputActions.Player.Crouch.IsPressed() == false)
            {
                IsSliding = false;
                return;
            }

            IsSliding = true;
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (IsFlying && CanDoWallJump() == false)
                return;

            Vector3 jumpDirection = Vector3.up;

            if (!IsGroundend)
            {
                if (IsSurfaceOnGivenDirection(-transform.right))
                    jumpDirection += (transform.right + transform.forward) * _config.WallJumpForce * Time.deltaTime;
                else if (IsSurfaceOnGivenDirection(transform.right))
                    jumpDirection += (-transform.right + transform.forward) * _config.WallJumpForce * Time.deltaTime;
            }

            Velocity += jumpDirection * Mathf.Sqrt(_config.JumpForce * -2f * GRAVITY);
            _characterController.Move(Velocity * Time.deltaTime);
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
    }
}