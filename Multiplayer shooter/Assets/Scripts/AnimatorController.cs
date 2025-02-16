using Game.Input;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private const string MOVE_X = "moveX";
    private const string MOVE_Y = "moveY";
    private const string IS_FLYING = "IsFlying";
    private float ANIMATION_CHANGE_SPEED = 0.3f;

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
        SetFlyingBool();
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
