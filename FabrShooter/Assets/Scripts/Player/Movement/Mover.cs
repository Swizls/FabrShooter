using FabrShooter.Input;
using UnityEngine;

namespace FabrShooter.Player.Movement
{
    public abstract class Mover
    {
        private const float GROUND_DETECTION_DISTANCE = 1.3f;

        private Transform _cameraTransform;
        private PlayerInputActions _playerInputActions;
        protected PlayerMovement PlayerMovement;

        protected Vector3 CalculatedVelocity;

        protected PlayerConfigSO Config => PlayerMovement.Config;
        protected CharacterController CharacterController => PlayerMovement.CharacterController;

        public Vector3 MovementDirection { get; private set; }
        public bool HasInput => MovementDirection.magnitude > 0;

        public Mover(PlayerMovement playerMovement, PlayerInputActions playerInputActions, Transform cameraTransform)
        {
            PlayerMovement = playerMovement;
            _playerInputActions = playerInputActions;
            _cameraTransform = cameraTransform;
        }

        public abstract void Move();

        public void ListenMovementInput()
        {
            Vector2 playerInput = _playerInputActions.Player.Move.ReadValue<Vector2>();

            Vector3 forward = _cameraTransform.TransformDirection(Vector3.forward);
            Vector3 right = _cameraTransform.TransformDirection(Vector3.right);

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            MovementDirection = forward * playerInput.y + right * playerInput.x;
        }

        protected Vector3 AdjustVelocityToSlope(Vector3 velocity)
        {
            Ray ray = new Ray(PlayerMovement.transform.position, Vector3.down);

            if (!Physics.Raycast(ray, out RaycastHit hitInfo, GROUND_DETECTION_DISTANCE, LayerMask.GetMask("Default")))
                return velocity;

            Vector3 projectedVector = Vector3.ProjectOnPlane(velocity, hitInfo.normal);

            if (projectedVector.y > 0)
                projectedVector.y -= 10f;

            return projectedVector;
        }

        protected void ApplyGravity()
        {
            float gravityY = PlayerMovement.IsFlying ? Physics.gravity.y * Time.deltaTime : Physics.gravity.y;
            Vector3 gravityForce = new Vector3(0, gravityY, 0);

            CalculatedVelocity += gravityForce;
        }
    }

}