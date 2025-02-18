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

        private void TogglePauseMenu(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Cursor.lockState = PauseMenu.gameObject.activeInHierarchy ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = PauseMenu.gameObject.activeInHierarchy;

            PauseMenu.gameObject.SetActive(!PauseMenu.gameObject.activeInHierarchy);
        }

        public void Disconnect()
        {
            GameManager gameManager = FindAnyObjectByType<GameManager>();

            gameManager.EndGame();
        }
    }
}