using FabrShooter.Input;
using UnityEngine;

namespace FabrShooter.Player.Movement
{
    public abstract class Mover
    {
        protected PlayerMovement PlayerMovement;
        private PlayerInputActions _playerInputActions;
        private Camera _camera;

        protected Vector3 CalculatedVelocity;

        protected PlayerConfigSO Config => PlayerMovement.Config;
        protected CharacterController CharacterController => PlayerMovement.CharacterController;

        public Vector3 MovementDirection { get; private set; }

        public Mover(PlayerMovement playerMovement, PlayerInputActions playerInputActions, Camera camera)
        {
            PlayerMovement = playerMovement;
            _playerInputActions = playerInputActions;
            _camera = camera;
        }

        public abstract void Move();

        public void ListenMovementInput()
        {
            Vector2 playerInput = _playerInputActions.Player.Move.ReadValue<Vector2>();

            Vector3 forward = _camera.transform.TransformDirection(Vector3.forward);
            Vector3 right = _camera.transform.TransformDirection(Vector3.right);

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            MovementDirection = forward * playerInput.y + right * playerInput.x;
        }
    }

}