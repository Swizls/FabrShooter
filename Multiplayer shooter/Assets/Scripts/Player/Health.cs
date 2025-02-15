using System;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    private const int DEFAULT_HEALTH = 100;

    private NetworkVariable<int> _value = new NetworkVariable<int>(DEFAULT_HEALTH);

    public Action OnDeath;

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int damage)
    {
        if (damage < 0 || _value.Value <= 0)
            return;

        _value.Value -= damage;
        Debug.Log($"Client: {OwnerClientId}; Got damage;");

        if (_value.Value <= 0)
            Die();
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
