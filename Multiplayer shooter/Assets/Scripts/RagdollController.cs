using System;
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

        public event Action OnRagdollEnable;
        public event Action OnRagdollDisable;

        public bool IsRagdollActive { get; private set; }

        public override void OnNetworkSpawn()
        {
            if (_root == null)
                throw new NullReferenceException("Rig root is not setted");

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

            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
            }

            IsRagdollActive = false;

            OnRagdollDisable?.Invoke();
        }

        [ClientRpc]
        public void EnableRagdollClientRpc()
        {
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

            IsRagdollActive = true;

            OnRagdollEnable?.Invoke();
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestDisableRagdollServerRpc()
        {
            DisableRagdollClientRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestEnableRagdollServerRpc()
        {
            EnableRagdollClientRpc();
        }
    }
}
