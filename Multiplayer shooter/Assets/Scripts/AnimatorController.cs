using Game.Input;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Animator _animator;
    private PlayerInput _playerInputActions;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();

        _playerInputActions = new PlayerInput();

        _playerInputActions.Player.Enable();
    }


    private void Update()
    {
        SetMovementDirection();
    }

    private void SetMovementDirection()
    {
        Vector2 dir = _playerInputActions.Player.Move.ReadValue<Vector2>();

        _animator.SetFloat("moveX", dir.x);
        _animator.SetFloat("moveY", dir.y);
    }

}
