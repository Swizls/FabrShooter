using FabrShooter.Input;
using FabrShooter.Player;
using System;
using UnityEngine;

namespace FabrShooter
{
    public class AnimatorController : MonoBehaviour, IPlayerInitializableComponent
    {
        private const string MOVE_X = "moveX";
        private const string MOVE_Y = "moveY";
        private const string IS_FLYING = "IsFlying";
        private const string IS_RUNNING = "IsRunning";
        private const string PUNCH = "Punch";
        private const string IS_SLIDING = "IsSliding";

        private float ANIMATION_CHANGE_SPEED = 0.1f;

        private PlayerMovement _playerMovement;
        private PlayerAttack _playerAttack;
        private Animator _animator;

        public void InitializeLocalPlayer()
        {
            _animator = GetComponentInChildren<Animator>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAttack = GetComponent<PlayerAttack>();

            _playerAttack.OnPunch += ActivateTriggerPunch;
        }

        public void InitializeClientPlayer()
        {
            Destroy(this);
        }

        private void OnEnable()
        {

            if(_playerAttack != null)
                _playerAttack.OnPunch += ActivateTriggerPunch;
        }

        private void OnDisable()
        {
            if(_playerAttack != null)
                _playerAttack.OnPunch -= ActivateTriggerPunch;
        }

        private void Update()
        {
            SetMovementDirection();
            SetFlyingBool();
            SetIsRunningBool();
            SetIsSlidingBool();
        }

        private void SetIsSlidingBool()
        {
            _animator.SetBool(IS_SLIDING, _playerMovement.IsSliding);
        }

        private void SetIsRunningBool()
        {
            _animator.SetBool(IS_RUNNING, _playerMovement.IsRunning);
        }

        private void ActivateTriggerPunch()
        {
            _animator.SetTrigger(PUNCH);
        }

        private void SetFlyingBool()
        {
            _animator.SetBool(IS_FLYING, _playerMovement.IsFlying);
        }

        private void SetMovementDirection()
        {
            _animator.SetFloat(MOVE_X, Mathf.Lerp(_animator.GetFloat(MOVE_X), _playerMovement.InputDirection.x, ANIMATION_CHANGE_SPEED));
            _animator.SetFloat(MOVE_Y, Mathf.Lerp(_animator.GetFloat(MOVE_Y), _playerMovement.InputDirection.y, ANIMATION_CHANGE_SPEED));
        }
    }
}