using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        [SerializeField] private List<Behaviour> _componentsToDisableOnDeath;

        private Health _health;
        private RagdollController _ragdollController;

        private void Start()
        {
            _health = GetComponent<Health>();
            _ragdollController = GetComponentInChildren<RagdollController>();

            _health.OnDeath += OnPlayerDeath;
        }

        private void OnDisable()
        {
            _health.OnDeath -= OnPlayerDeath;
        }

        private void OnPlayerDeath(ulong clientID)
        {
            foreach (var component in _componentsToDisableOnDeath)
                component.enabled = false;

            _ragdollController.EnableRagdollClientRpc();

            StartCoroutine(DelayRespawnRequest(clientID));
        }

        private IEnumerator DelayRespawnRequest(ulong clientID)
        {
            float timer = 5f;
            Debug.Log($"Respawn timer started for client({clientID})");
            while (timer >= 0)
            {
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            GetComponentInChildren<Camera>().gameObject.SetActive(false);

            RequestRespawn(clientID);
        }

        private void RequestRespawn(ulong clientID)
        {
            PlayerSpawner spawner = FindAnyObjectByType<PlayerSpawner>();

            spawner.RespawnPlayerServerRpc(clientID);
        }
    }
}