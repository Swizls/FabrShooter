using UnityEngine;

namespace FabrShooter.Player
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "Configs/Player")]
    public class PlayerConfigSO : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float _walkingSpeed;
        [SerializeField] private float _spritingMultiplier;
        [SerializeField][Range(0f, 1f)] private float _movementInertia;
        [Space]
        [SerializeField] private float _maxStamina;
        [SerializeField] private float _staminaConsumptionSpeed;
        [SerializeField] private float _staminaRestoreSpeed;
        [Space]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _wallJumpForce;
        [SerializeField][Range(0f, 1f)] private float _jumpInertia;
        [Space]
        [SerializeField][Range(1f, 2f)] private float _slopeAcceleration;
        [SerializeField][Range(0f, 1f)] private float _slopeDecceleration;
        [SerializeField][Range(0f, 1f)] private float _slideInertia;

        [Space]
        [Header("Camera")]
        [SerializeField][Range(0f, 120f)] private float _maxYAngle;

        public float WalkingSpeed => _walkingSpeed;
        public float SprintingMultiplier => _spritingMultiplier;
        public float MovementInertia => _movementInertia;
        public float JumpForce => _jumpForce;
        public float WallJumpForce => _wallJumpForce;
        public float JumpInertia => _jumpInertia;
        public float SlopeAcceleration => _slopeAcceleration;
        public float SlopeDecceleration => _slopeDecceleration;
        public float SlideInertia => _slideInertia;

        public float MaxStamina => _maxStamina;
        public float StaminaConsumptionSpeed => _staminaConsumptionSpeed;
        public float StaminaRestoreSpeed => _staminaRestoreSpeed;

        public float MaxYAngle => _maxYAngle;
    }
}