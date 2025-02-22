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
            throw new System.NotImplementedException();
        }
    }
}