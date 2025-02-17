using Unity.Netcode;
using UnityEngine;

namespace FabrShooter 
{
    [RequireComponent(typeof(NetworkObject))]
    public class RagdollController : NetworkBehaviour
    {
        [SerializeField] private GameObject _root;

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
            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
            }
        }

        [ClientRpc]
        public void EnableRagdollClientRpc()
        {
            foreach (var rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
            }
        }
    }
}
