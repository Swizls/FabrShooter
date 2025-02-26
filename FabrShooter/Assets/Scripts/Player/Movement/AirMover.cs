using FabrShooter.Input;
using UnityEngine;

namespace FabrShooter.Player.Movement
{
    public class AirMover : Mover
    {
        public AirMover(PlayerMovement playerMovement,
            PlayerInputActions playerInputActions,
            Camera camera) : base(playerMovement, playerInputActions, camera) { }

        public override void Move()
        {
            float speed = Config.WalkingSpeed * Config.SprintingMultiplier;

            Vector3 forwardComponent = Vector3.Project(MovementDirection, CharacterController.velocity.normalized);
            Vector3 sideComponent = MovementDirection - forwardComponent;

            forwardComponent *= Config.JumpInertia;
            Vector3 CalculatedMovementDirection = forwardComponent + sideComponent;

            CalculatedVelocity.x = Mathf.MoveTowards(CharacterController.velocity.x, CalculatedMovementDirection.x * speed, Config.MovementInertia);
            CalculatedVelocity.y = PlayerMovement.Velocity.y;
            CalculatedVelocity.z = Mathf.MoveTowards(CharacterController.velocity.z, CalculatedMovementDirection.z * speed, Config.MovementInertia);

            ApplyGravity();
            CharacterController.Move(CalculatedVelocity * Time.deltaTime);
        }
    }
}