using System;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter
{
    public class Health : NetworkBehaviour
    {
        private const int DEFAULT_HEALTH = 100;

        private int _value = DEFAULT_HEALTH;

        public Action OnValueChange;
        public Action<ulong> OnDeath;

        public int Value => _value;
        public bool IsDead => _value <= 0;

        [ClientRpc]
        public void TakeDamageClientRpc(int damage)
        {
            if (damage < 0 || _value <= 0)
                return;

            _value -= damage;

            OnValueChange?.Invoke();

            if (_value <= 0)
                Die();
        }

        private void Die()
        {
            Debug.Log($"Client: {OwnerClientId}; Died;");

            if (_value < 0)
                _value = 0;

            OnDeath?.Invoke(OwnerClientId);
        }
    }
}
