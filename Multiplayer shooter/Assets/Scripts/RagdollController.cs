using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter 
{
    [RequireComponent(typeof(NetworkObject))]
    public class RagdollController : NetworkBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private List<Behaviour> _componentsToDisableOnRegdoll;

        private Rigidbody[] _rigidbodies;

        public override void OnNetworkSpawn()
        {
            if (_root == null)
                throw new System.NullReferenceException("Rig root is not setted");

            _rigidbodies = _root.GetComponentsInChildren<Rigidbody>();

            DisableRagdollClientRpc();
        }

        [ClientRpc]
        public void DisableRagdollClientRpc()
        {
            foreach (var component in _componentsToDisableOnRegdoll)
            {
                if (component == null)
                    continue;

                component.enabled = true;
            }

            Debug.Log($"Ragdoll disabled for client({OwnerClientId})");

            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
            }
        }

        [ClientRpc]
        public void EnableRagdollClientRpc()
        {
            Debug.Log($"Ragdoll enabled for client({OwnerClientId})");

            foreach (var component in _componentsToDisableOnRegdoll)
            {
                if (component == null)
                    continue;

                component.enabled = false;
            }

            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
            }
        }

        [ServerRpc]
        public void RequestDisableRagdollServerRpc()
        {
            DisableRagdollClientRpc();
        }

        [ServerRpc]
        public void RequestEnableRagdollServerRpc()
        {
            EnableRagdollClientRpc();
        }
    }
}
