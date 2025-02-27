using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter 
{
    [RequireComponent(typeof(NetworkObject))]
    public class ServerDamageDealer : NetworkBehaviour
    {
        public event Action<AttackData> OnDamageDealing;

        [ServerRpc(RequireOwnership = false)]
        public void DealDamageServerRpc(AttackData data)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(data.TargetID, out NetworkObject targetObject))
            {
                Debug.Log($"Network object with ID: {data.TargetID} is not found");
                return;
            }

            var health = targetObject.GetComponent<Health>();
            var hitboxController = targetObject.GetComponentInChildren<HitboxHitHandler>();

            health.TakeDamageClientRpc(data.Damage);
            hitboxController.RegisterHitClientRpc(data.TargetID, data.HitboxID);

            Debug.Log($"Server is dealing damage to client({targetObject.OwnerClientId}); Damage: {data.Damage}; Knockback: {data.UseKnockback}");

            if(data.TryGetSenderID(out ulong senderID) )
            {
                NotifyClientsAboutDealtDamageClientRpc(data, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { senderID } } });
            }

            if (!data.UseKnockback)
                return;

            if (targetObject.TryGetComponent(out KnockbackController knockbackController))
                knockbackController.ApplyKnockbackClientRpc(data.KnockbackForce);
        }

        [ClientRpc]
        private void NotifyClientsAboutDealtDamageClientRpc(AttackData data, ClientRpcParams rpcParams)
        {
            OnDamageDealing?.Invoke(data);
        }
    }
}