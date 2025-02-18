using Unity.Netcode;
using UnityEngine;

namespace FabrShooter 
{
    [RequireComponent(typeof(NetworkObject))]
    public class ServerDamageDelaer : NetworkBehaviour
    {
        [ServerRpc(RequireOwnership = false)]
        public void DealDamageServerRpc(AttackData data)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(data.TargetID, out NetworkObject targetObject))
                return;

            if (targetObject.TryGetComponent(out Health health))
                health.TakeDamageClientRpc(data.Damage);

            if (!data.UseKnockback)
                return;

            if (targetObject.TryGetComponent(out KnockbackController knockbackController))
                knockbackController.ApplyKnockbackClientRpc(data.KnockbackForce);
        }
    }
}