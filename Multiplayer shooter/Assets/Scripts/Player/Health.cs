using FabrShooter.UI;
using System;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter
{
    public class Health : NetworkBehaviour
    {
        private const int DEFAULT_HEALTH = 100;

        private NetworkVariable<int> _value = new NetworkVariable<int>(DEFAULT_HEALTH);

        public Action OnValueChange;
        public Action<ulong> OnDeath;

        public int Value => _value.Value;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            HealthUI healthUI = FindFirstObjectByType<HealthUI>();

            healthUI.Initialize(this, OwnerClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        public void TakeDamageServerRpc(int damage)
        {
            if (damage < 0 || _value.Value <= 0)
                return;

            _value.Value -= damage;
            Debug.Log($"Client: {OwnerClientId}; Got damage; Health: {_value.Value}");

            OnValueChange?.Invoke();

            if (_value.Value <= 0)
                Die();
        }

        private void Die()
        {
            Debug.Log($"Client: {OwnerClientId}; Died;");
            OnDeath?.Invoke(OwnerClientId);
        }
    }
}
