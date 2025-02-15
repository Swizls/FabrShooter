using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "Configs/Player")]
    public class PlayerConfigSO : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField][Range(0f, 30f)] private float _walkingSpeed;
        [SerializeField][Range(1f, 3f)] private float _spritingMultiplier;
        [Space]
        [SerializeField] private float _maxStamina;
        [SerializeField] private float _staminaConsumptionSpeed;
        [SerializeField] private float _staminaRestoreSpeed;
        [Space]
        [SerializeField] private float _jumpForce;

        [Space]
        [Header("Camera")]
        [SerializeField][Range(0f, 150f)] private float _sensitivity;
        [SerializeField][Range(0f, 120f)] private float _maxYAngle;

        public float WalkingSpeed => _walkingSpeed;
        public float SprintingMultiplier => _spritingMultiplier;
        public float JumpForce => _jumpForce;

        public float MaxStamina => _maxStamina;
        public float StaminaConsumptionSpeed => _staminaConsumptionSpeed;
        public float StaminaRestoreSpeed => _staminaRestoreSpeed;

        public float Sensitivity => _sensitivity;
        public float MaxYAngle => _maxYAngle;
    }
}