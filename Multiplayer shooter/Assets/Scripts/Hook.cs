using Game.Input;
using System.Collections;
using UnityEngine;

namespace FabrShooter
{
    public class Hook : MonoBehaviour, IPlayerInitializableComponent
    {
        [SerializeField] private float _hookSpeed;

        private PlayerMovement _playerMovement;
        private PlayerInput _playerInput;

        private Transform _cameraTransform;

        public void Initialize()
        {
            _playerMovement = GetComponent<PlayerMovement>();

            _cameraTransform = GetComponentInChildren<Camera>().transform;

            _playerInput = new PlayerInput();
            _playerInput.Player.Enable();
            _playerInput.Player.Hook.performed += StartHook;
        }

        private void OnEnable()
        {
            if (_playerInput == null)
                return;

            _playerInput.Player.Hook.performed += StartHook;
            _playerInput.Player.Enable();
        }

        private void OnDisable()
        {
            if (_playerInput == null)
                return;

            _playerInput.Player.Hook.performed -= StartHook;
            _playerInput.Player.Disable();
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

            while (_playerInput.Player.Hook.IsPressed())
            {
                if (IsDistanceReached() == false)
                    transform.position = Vector3.MoveTowards(transform.position, hitPostion, _hookSpeed * Time.deltaTime);

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