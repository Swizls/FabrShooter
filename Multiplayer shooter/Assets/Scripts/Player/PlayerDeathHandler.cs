using System.Collections;
using UnityEngine;

namespace FabrShooter
{
    public class PlayerDeathHandler : MonoBehaviour
    {
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
            Debug.Log($"Disabling health component. Health is {_health}");
            if(_health == null)
                _health.OnDeath -= OnPlayerDeath;
        }

        private void OnPlayerDeath(ulong clientID)
        {
            Debug.Log($"OnPlayerDeath() invoke (client: {clientID})");

            _ragdollController.RequestEnableRagdollServerRpc();

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