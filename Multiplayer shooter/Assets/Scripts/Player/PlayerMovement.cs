using System;
using Game.Player;
using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

namespace Game.Input
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : NetworkBehaviour
    {
        private const float FALL_SPEED = 1f;

        [SerializeField] private PlayerConfigSO _config;
        [SerializeField] private Camera _camera;

        private CharacterController _characterController;
        private PlayerInput _playerInputActions;
        private Health _health;

        private Vector3 _movementDirection;

        private float _stamina;
        private bool _isJumping;

        public Action StartedStaminaConsumption;
        public Action EndedStaminaConsumtion;

        public float Stamina => _stamina;
        public PlayerConfigSO Config => _config;
        public Vector3 MovementDirection => _movementDirection;

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
            get { return _characterController.velocity.magnitude > 0; }
        }

        #region MONO
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _health = GetComponent<Health>();
            _playerInputActions = new PlayerInput();

            _playerInputActions.Player.Enable();
            _playerInputActions.Player.Jump.performed += Jump;

            _stamina = _config.MaxStamina;

            _health.OnDeath += OnPlayerDeath;
        }

        private void OnDisable()
        {
            _health.OnDeath -= OnPlayerDeath;
            _characterController.Move(Vector3.zero);
            _playerInputActions.Player.Jump.performed -= Jump;
        }

        private void OnPlayerDeath()
        {
            this.enabled = false;
        }

        private void Update()
        {
            if (!IsOwner)
            {
                Destroy(this);
                return;
            }

            ListenMovementInput();
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return;

            Move();
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, _movementDirection);
            Gizmos.DrawRay(transform.position, transform.forward);
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

            _movementDirection.y = _characterController.isGrounded || !_isJumping ? _movementDirection.y : -FALL_SPEED;

            _characterController.Move(_movementDirection * speed);
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (_characterController.isGrounded == false && _isJumping)
                return;

            _isJumping = true;

            _movementDirection.y = 1;

            StartCoroutine(JumpCoroutine());
        }

        private IEnumerator JumpCoroutine()
        {
            float timer = 2f;

            while (timer > 0)
            {
                _movementDirection.y = 1;
                timer -= Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            _isJumping = false;
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

            _movementDirection = (forward * playerInput.y) + (right * playerInput.x);
        }
    }
}