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

            Vector3 forwardComponent = Vector3.Project(MovementDirection, PlayerMovement.Velocity.normalized);
            Vector3 sideComponent = MovementDirection - forwardComponent;

            forwardComponent *= Config.JumpInertia;
            Vector3 CalculatedMovementDirection = forwardComponent + sideComponent;

            PlayerMovement.Velocity.x = Mathf.MoveTowards(PlayerMovement.Velocity.x, CalculatedMovementDirection.x * speed, Config.MovementInertia);
            PlayerMovement.Velocity.z = Mathf.MoveTowards(PlayerMovement.Velocity.z, CalculatedMovementDirection.z * speed, Config.MovementInertia);

            CharacterController.Move(PlayerMovement.Velocity * Time.deltaTime);
        }
    }
}