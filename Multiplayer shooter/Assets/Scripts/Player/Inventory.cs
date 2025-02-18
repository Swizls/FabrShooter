using UnityEngine;

namespace FabrShooter 
{ 
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private WeaponSO _currentWeapon;

        public WeaponSO CurrentWeapon => _currentWeapon;
    }
}