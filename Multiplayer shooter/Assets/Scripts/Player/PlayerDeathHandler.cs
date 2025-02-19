using System.Collections;
using UnityEngine;

namespace FabrShooter
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerDeathHandler : MonoBehaviour, IPlayerInitializableComponent
    {
        [SerializeField] private AudioClip[] _deathSounds; 

        private Health _health;
        private RagdollController _ragdollController;
        private KnockbackController _knockbackController;
        private AudioSource _audioSource;

        public void Initialize()
        {
            _health = GetComponent<Health>();
            _ragdollController = GetComponentInChildren<RagdollController>();
            _knockbackController = GetComponent<KnockbackController>();
            _audioSource = GetComponent<AudioSource>();

            _health.OnDeath += OnPlayerDeath;
        }

        private void OnDisable()
        {
            if(_health != null)
                _health.OnDeath -= OnPlayerDeath;
        }

        private void OnPlayerDeath(ulong clientID)
        {
            Debug.Log($"OnPlayerDeath() invoke (client: {clientID})");

            _knockbackController.enabled = false;
            _ragdollController.RequestEnableRagdollServerRpc();
            _audioSource.PlayOneShot(_deathSounds[Random.Range(0, _deathSounds.Length)]);

            StartCoroutine(DelayRespawnRequest(clientID));
        }

        private IEnumerator DelayRespawnRequest(ulong clientID)
        {
            float timer = 5f;
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