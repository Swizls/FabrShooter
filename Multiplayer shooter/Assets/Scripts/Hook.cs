using FabrShooter.Input;
using FabrShooter.Player;
using System.Collections;
using UnityEngine;

namespace FabrShooter
{
    public class Hook : MonoBehaviour, IPlayerInitializableComponent
    {
        [SerializeField] private float _hookSpeed;

        private PlayerMovement _playerMovement;
        private PlayerInputActions _playerInputActions;

        private Transform _cameraTransform;

        public void InitializeLocalPlayer()
        {
            _playerMovement = GetComponent<PlayerMovement>();

            _cameraTransform = GetComponentInChildren<Camera>().transform;

            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
            _playerInputActions.Player.Hook.performed += StartHook;
        }

        public void InitializeClientPlayer()
        {
            Destroy(this);
        }

        private void OnEnable()
        {
            if (_playerInputActions == null)
                return;

            _playerInputActions.Player.Hook.performed += StartHook;
            _playerInputActions.Player.Enable();
        }

        private void OnDisable()
        {
            if (_playerInputActions == null)
                return;

            _playerInputActions.Player.Hook.performed -= StartHook;
            _playerInputActions.Player.Disable();
        }

        private void StartHook(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit))
                return;

            StartCoroutine(StartHook(hit.point));
        }


        private IEnumerator StartHook(Vector3 hitPostion)
        {
            _playerMovement.enabled = false;

            Vector3 hookDirection = _cameraTransform.forward;

            while (_playerInputActions.Player.Hook.IsPressed())
            {
                if (IsDistanceReached() == false)
                    _playerMovement.CharacterController.Move(hookDirection * _hookSpeed * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }

            _playerMovement.enabled = true;

            bool IsDistanceReached()
            {
                const float MIN_DISTANCE_TO_STOP = 1f;
                return Vector3.Distance(transform.position, hitPostion) < MIN_DISTANCE_TO_STOP;
            }
        }
    }
}