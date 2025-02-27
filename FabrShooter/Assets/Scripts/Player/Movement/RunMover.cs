using FabrShooter.Input;
using UnityEngine;

namespace FabrShooter.Player.Movement
{
    public class RunMover : Mover
    {
        public RunMover(PlayerMovement playerMovement, PlayerInputActions playerInputActions, Transform cameraTransform) : base(playerMovement, playerInputActions, cameraTransform) {}

        public override void Move()
        {
            CalculatedVelocity.x = Mathf.MoveTowards(PlayerMovement.Velocity.x, MovementDirection.x * Config.WalkingSpeed * Config.SprintingMultiplier, Config.MovementInertia);
            CalculatedVelocity.z = Mathf.MoveTowards(PlayerMovement.Velocity.z, MovementDirection.z * Config.WalkingSpeed * Config.SprintingMultiplier, Config.MovementInertia);

            CalculatedVelocity.y = PlayerMovement.Velocity.y;

            CalculatedVelocity = AdjustVelocityToSlope(CalculatedVelocity);

            ApplyGravity();
            CharacterController.Move(CalculatedVelocity * Time.deltaTime);
        }
    }
}