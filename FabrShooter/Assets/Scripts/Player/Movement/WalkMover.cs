using FabrShooter.Input;
using UnityEngine;

namespace FabrShooter.Player.Movement
{
    public class WalkMover : Mover
    {
        public WalkMover(PlayerMovement playerMovement, 
            PlayerInputActions playerInputActions, 
            Camera camera) : base(playerMovement, playerInputActions, camera) { }

        public override void Move()
        {
            CalculatedVelocity.x = Mathf.MoveTowards(PlayerMovement.Velocity.x, MovementDirection.x * Config.WalkingSpeed, Config.MovementInertia);
            CalculatedVelocity.z = Mathf.MoveTowards(PlayerMovement.Velocity.z, MovementDirection.z * Config.WalkingSpeed, Config.MovementInertia);

            CalculatedVelocity.y = PlayerMovement.Velocity.y;

            CalculatedVelocity = AdjustVelocityToSlope(CalculatedVelocity);

            ApplyGravity();
            CharacterController.Move(CalculatedVelocity * Time.deltaTime);
        }
    }
}