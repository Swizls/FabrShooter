using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter 
{
    public class PlayerComponentsController : NetworkBehaviour
    {
        [SerializeField] private List<Behaviour> _componentsToDisableOnDeath;

        private Health _health;

        private RagdollController _ragdollController;

        private void Start()
        {
            _health = GetComponent<Health>();
            _ragdollController = GetComponentInChildren<RagdollController>();

            _health.OnDeath += OnPlayerDeathClientRpc;
        }

        private void OnDisable()
        {
            _health.OnDeath -= OnPlayerDeathClientRpc;
        }

        [ClientRpc]
        private void OnPlayerDeathClientRpc(ulong clientID)
        {
            foreach (var component in _componentsToDisableOnDeath)
                component.enabled = false;

            _ragdollController.EnableRagdoll();
        }
    }
}
