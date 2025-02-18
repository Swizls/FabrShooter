using FabrShooter.Input;
using FabrShooter.Player;
using UnityEngine;

namespace FabrShooter
{
    public class AnimatorController : MonoBehaviour, IPlayerInitializableComponent
    {
        private const string MOVE_X = "moveX";
        private const string MOVE_Y = "moveY";
        private const string IS_FLYING = "IsFlying";
        private float ANIMATION_CHANGE_SPEED = 0.3f;

        private PlayerMovement _playerMovement;
        private PlayerAttack _playerAttack;
        private Animator _animator;
        private PlayerInputActions _playerInputActions;

        public void Initialize()
        {
            _animator = GetComponentInChildren<Animator>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAttack = GetComponent<PlayerAttack>();

            _playerAttack.OnPunch += ActivateTriggerPunch;

            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
        }

        private void OnEnable()
        {
            if (_playerInputActions != null)
                _playerInputActions.Player.Enable();

            if(_playerAttack != null)
                _playerAttack.OnPunch += ActivateTriggerPunch;
        }

        private void OnDisable()
        {
            if (_playerInputActions != null)
                _playerInputActions.Player.Disable();

            if(_playerAttack != null)
                _playerAttack.OnPunch -= ActivateTriggerPunch;
        }

        private void Update()
        {
            SetMovementDirection();
            SetFlyingBool();
        }

        private void ActivateTriggerPunch()
        {
            _animator.SetTrigger("Punch");
        }

        private void SetFlyingBool()
        {
            _animator.SetBool(IS_FLYING, _playerMovement.IsFlying);
        }

        private void SetMovementDirection()
        {
            Vector2 dir = _playerInputActions.Player.Move.ReadValue<Vector2>();

            _animator.SetFloat(MOVE_X, Mathf.Lerp(_animator.GetFloat(MOVE_X), dir.x, ANIMATION_CHANGE_SPEED));
            _animator.SetFloat(MOVE_Y, Mathf.Lerp(_animator.GetFloat(MOVE_Y), dir.y, ANIMATION_CHANGE_SPEED));
        }
    }
}