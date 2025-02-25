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
        public bool HasInput => MovementDirection.magnitude > 0;

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

        protected Vector3 AdjustVelocityToSlope(Vector3 velocity)
        {
            Ray ray = new Ray(PlayerMovement.transform.position, Vector3.down);

            if (!Physics.Raycast(ray, out RaycastHit hitInfo, PlayerMovement.GROUND_DETECTION_DISTANCE, LayerMask.GetMask("Default")))
                return velocity;

            return Vector3.ProjectOnPlane(velocity, hitInfo.normal);
        }
    }

}