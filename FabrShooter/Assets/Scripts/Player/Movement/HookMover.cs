using FabrShooter.Input;
using UnityEngine;

namespace FabrShooter.Player.Movement 
{
    public class HookMover : Mover
    {
        public HookMover(PlayerMovement playerMovement,
            PlayerInputActions playerInputActions,
            Transform cameraTransform) : base(playerMovement, playerInputActions, cameraTransform) { }

        public override void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}