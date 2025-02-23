using FabrShooter.Input;
using UnityEngine;

namespace FabrShooter.Player.Movement
{
    public abstract class Mover
    {
        protected PlayerMovement PlayerMovement;
        private PlayerInputActions _playerInputActions;
        protected Camera Camera;

        protected Vector3 CalculatedVelocity;

        protected PlayerConfigSO Config => PlayerMovement.Config;
        protected CharacterController CharacterController => PlayerMovement.CharacterController;

        public Vector3 MovementDirection { get; private set; }

        public Mover(PlayerMovement playerMovement, PlayerInputActions playerInputActions, Camera camera)
        {
            PlayerMovement = playerMovement;
            _playerInputActions = playerInputActions;
            Camera = camera;
        }

        public abstract void Move();

        public void ListenMovementInput()
        {
            Vector2 playerInput = _playerInputActions.Player.Move.ReadValue<Vector2>();

            Vector3 forward = Camera.transform.TransformDirection(Vector3.forward);
            Vector3 right = Camera.transform.TransformDirection(Vector3.right);

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            MovementDirection = forward * playerInput.y + right * playerInput.x;
        }
    }

}