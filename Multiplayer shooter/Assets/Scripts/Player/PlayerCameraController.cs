using FabrShooter.Player;
using UnityEngine;

namespace FabrShooter.Input
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private PlayerConfigSO _config;

        private Vector2 _currentRotation;

        private bool _canMoveCamera = true;

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (!_canMoveCamera)
                return;

            if (Cursor.lockState != CursorLockMode.Locked)
                return;

            float mouseX = UnityEngine.Input.GetAxis("Mouse X") * _config.Sensitivity;
            float mouseY = UnityEngine.Input.GetAxis("Mouse Y") * _config.Sensitivity;

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