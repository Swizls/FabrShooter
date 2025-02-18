using UnityEngine;

namespace FabrShooter
{
    [CreateAssetMenu(fileName = "Weapon data", menuName = "Weapons")]
    public class WeaponSO : ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] private bool _useKnockback;

        public int Damage => _damage;
        public bool UseKnockback => _useKnockback;
    }
}