using FabrShooter.Input;
using UnityEngine;

namespace FabrShooter.Player.Movement
{
    public class RunMover : Mover
    {
        public RunMover(PlayerMovement playerMovement, PlayerInputActions playerInputActions, Camera camera) : base(playerMovement, playerInputActions, camera) {}

        public override void Move()
        {
            CalculatedVelocity.x = Mathf.MoveTowards(PlayerMovement.Velocity.x, MovementDirection.x * Config.WalkingSpeed * Config.SprintingMultiplier, Config.MovementInertia);
            CalculatedVelocity.z = Mathf.MoveTowards(PlayerMovement.Velocity.z, MovementDirection.z * Config.WalkingSpeed * Config.SprintingMultiplier, Config.MovementInertia);

            PlayerMovement.Velocity.x = CalculatedVelocity.x;
            PlayerMovement.Velocity.z = CalculatedVelocity.z;

            CharacterController.Move(PlayerMovement.Velocity * Time.deltaTime);
        }
    }
}