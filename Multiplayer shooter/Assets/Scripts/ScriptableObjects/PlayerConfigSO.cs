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
        [SerializeField][Range(0f, 1f)] private float _jumpInertia;

        [Space]
        [Header("Camera")]
        [SerializeField][Range(0f, 150f)] private float _sensitivity;
        [SerializeField][Range(0f, 120f)] private float _maxYAngle;

        public float WalkingSpeed => _walkingSpeed;
        public float SprintingMultiplier => _spritingMultiplier;
        public float MovementInertia => _movementInertia;
        public float JumpForce => _jumpForce;
        public float JumpInertia => _jumpInertia;

        public float MaxStamina => _maxStamina;
        public float StaminaConsumptionSpeed => _staminaConsumptionSpeed;
        public float StaminaRestoreSpeed => _staminaRestoreSpeed;

        public float Sensitivity => _sensitivity;
        public float MaxYAngle => _maxYAngle;
    }
}