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
            {
                Debug.Log($"Network object with ID: {data.TargetID} is not found");
                return;
            }

            var health = targetObject.GetComponent<Health>();
            var hitboxController = targetObject.GetComponentInChildren<HitboxController>();

            Debug.Log($"GameObject {targetObject.gameObject}; Target ID {data.TargetID}; Hitbox ID: {data.HitboxID}; Health: {health}; HitboxController: {hitboxController}");
            
            health.TakeDamageClientRpc(data.Damage);
            hitboxController.RegisterHitClientRpc(data.HitboxID);

            Debug.Log($"Server is dealing damage to client({data.TargetID}); Damage: {data.Damage}; Knockback: {data.UseKnockback}");

            if (!data.UseKnockback)
                return;

            if (targetObject.TryGetComponent(out KnockbackController knockbackController))
                knockbackController.ApplyKnockbackClientRpc(data.KnockbackForce);
        }
    }
}