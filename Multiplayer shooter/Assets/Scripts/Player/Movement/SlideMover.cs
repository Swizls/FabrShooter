using FabrShooter.Input;
using UnityEngine;

namespace FabrShooter.Player.Movement
{
    public class SlideMover : Mover
    {
        public SlideMover(PlayerMovement playerMovement,
            PlayerInputActions playerInputActions,
            Camera camera) : base(playerMovement, playerInputActions, camera) { }

        public override void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}
