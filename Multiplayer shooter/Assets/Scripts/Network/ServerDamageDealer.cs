using Unity.Netcode;
using UnityEngine;

namespace FabrShooter 
{
    [RequireComponent(typeof(NetworkObject))]
    public class ServerDamageDelaer : NetworkBehaviour
    {
        [ServerRpc(RequireOwnership = false)]
        public void DealDamageServerRpc(ulong clientID, int damage)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(clientID, out NetworkObject targetObject))
                return;

            if (targetObject.TryGetComponent(out Health health))
                health.TakeDamageServerRpc(damage);

            Debug.Log($"Perform damage deal to client({clientID}); Damage: {damage}");
        }
    }
}