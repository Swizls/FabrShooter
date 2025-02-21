using FabrShooter.Player;
using UnityEngine;

namespace FabrShooter.Input
{
    public class PlayerCamera : MonoBehaviour, IPlayerInitializableComponent
    {
        [SerializeField] private PlayerConfigSO _config;
        [SerializeField] private Transform _cameraAnchor;

        private PlayerInputActions _playerInputActions;

        private Vector2 _currentRotation;

        private bool _canMoveCamera = true;

        public void InitializeLocalPlayer()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();

            if (!TryGetComponent(out Camera camera))
                return;

            camera.enabled = true;
            GetComponent<AudioListener>().enabled = true;

            enabled = true;
        }

        private void Update()
        {
            if (_cameraAnchor == null)
                return;

            transform.position = _cameraAnchor.position;

            if (!_canMoveCamera)
                return;

            if (Cursor.lockState != CursorLockMode.Locked)
                return;

            Vector2 mouseInput = _playerInputActions.Player.Look.ReadValue<Vector2>();

            float mouseX = mouseInput.x * _config.Sensitivity * Time.deltaTime;
            float mouseY = mouseInput.y * _config.Sensitivity * Time.deltaTime;

            _currentRotation.x += mouseX;
            _currentRotation.y -= mouseY;

            _currentRotation.y = Mathf.Clamp(_currentRotation.y, -_config.MaxYAngle, _config.MaxYAngle);

            transform.rotation = Quaternion.Euler(_currentRotation.y, _currentRotation.x, 0f);
        }

        public void OnPauseDisable()
        {
            _canMoveCamera = true;
        }

        public void OnPauseEnable()
        {
            _canMoveCamera = false;
        }
    }
}