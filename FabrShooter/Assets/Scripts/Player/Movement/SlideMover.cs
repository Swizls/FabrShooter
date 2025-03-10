﻿using FabrShooter.Input;
using UnityEngine;

namespace FabrShooter.Player.Movement
{
    public class SlideMover : Mover
    {
        private const float SLIDE_START_ACCELERATION = 5f;

        private Vector3 _slideDirection = Vector3.zero;

        private bool _isSlideStarted = false;

        public SlideMover(PlayerMovement playerMovement,
            PlayerInputActions playerInputActions,
            Camera camera) : base(playerMovement, playerInputActions, camera)
        { 
            _isSlideStarted = true;

            _slideDirection = camera.transform.forward;
            _slideDirection.y = 0;
            _slideDirection.Normalize();
        }

        public override void Move()
        {
            Vector3 horizontalVelocity = new Vector3(PlayerMovement.Velocity.x, 0, PlayerMovement.Velocity.z);
            float currentSpeed = horizontalVelocity.magnitude;

            float speed = Mathf.Max(currentSpeed - 2f * Time.deltaTime, 1f);

            Vector3 boostDirection = Vector3.zero;
            if (_isSlideStarted)
            {
                boostDirection = _slideDirection * SLIDE_START_ACCELERATION;
                speed += SLIDE_START_ACCELERATION;
                _isSlideStarted = false;
            }

            float verticalModifier = 1f;

            if (CharacterController.velocity.y > 0)
                verticalModifier = Config.SlopeDecceleration;
            else if (CharacterController.velocity.y < 0)
                verticalModifier = Config.SlopeAcceleration;

            speed *= verticalModifier;

            Vector3 finalDirection = (_slideDirection + boostDirection).normalized;

            PlayerMovement.Velocity.x = Mathf.MoveTowards(PlayerMovement.Velocity.x, finalDirection.x * speed, Config.MovementInertia);
            PlayerMovement.Velocity.z = Mathf.MoveTowards(PlayerMovement.Velocity.z, finalDirection.z * speed, Config.MovementInertia);

            CharacterController.Move(PlayerMovement.Velocity * Time.deltaTime);
        }
    }
}
