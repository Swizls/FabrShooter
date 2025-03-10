﻿using UnityEngine;

namespace FabrShooter
{
    [CreateAssetMenu(fileName = "Weapon data", menuName = "Weapons")]
    public class WeaponSO : ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] private bool _useKnockback;
        [SerializeField] private float _knockbackForce;
        [SerializeField] private AudioClip[] _sfx;

        public int Damage => _damage;
        public bool UseKnockback => _useKnockback;
        public float KnockbackForce => _knockbackForce;
        public AudioClip[] SFX => _sfx;
    }
}