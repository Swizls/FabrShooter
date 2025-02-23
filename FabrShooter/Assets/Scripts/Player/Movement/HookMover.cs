using FabrShooter.Input;
using UnityEngine;

namespace FabrShooter.Player.Movement 
{
    public class HookMover : Mover
    {
        public HookMover(PlayerMovement playerMovement,
            PlayerInputActions playerInputActions,
            Camera camera) : base(playerMovement, playerInputActions, camera) { }

        public override void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}