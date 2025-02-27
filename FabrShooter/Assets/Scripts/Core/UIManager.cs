using UnityEngine;
using FabrShooter.Core;
using FabrShooter.Input;

namespace FabrShooter.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private RectTransform PauseMenu;

        private PlayerInputActions _playerInputActions;

        private void Start()
        {
            _playerInputActions = new PlayerInputActions();

            _playerInputActions.UI.Enable();
            _playerInputActions.UI.Pause.performed += TogglePauseMenu;

            PauseMenu.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            ServiceLocator.Get<GameSessionManager>().OnGameStart += DisableCursor;
            ServiceLocator.Get<GameSessionManager>().OnGameStop += EnableCursor;

            if (_playerInputActions == null)
                return;

            _playerInputActions.UI.Enable();
            _playerInputActions.UI.Pause.performed += TogglePauseMenu;
        }

        private void OnDisable()
        {
            if (_playerInputActions == null)
                return;

            _playerInputActions.UI.Disable();
            _playerInputActions.UI.Pause.performed -= TogglePauseMenu;
        }

        private void OnDestroy()
        {
            EnableCursor();   
        }

        private void EnableCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void DisableCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void TogglePauseMenu(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            PauseMenu.gameObject.SetActive(!PauseMenu.gameObject.activeInHierarchy);

            if (PauseMenu.gameObject.activeInHierarchy)
                EnableCursor();
            else
                DisableCursor();
        }

        public void Disconnect()
        {
            ServiceLocator.Get<GameSessionManager>().EndGame();
        }
    }
}