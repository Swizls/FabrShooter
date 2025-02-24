using System.Collections;
using UnityEngine;

namespace FabrShooter
{
    [RequireComponent(typeof(NetworkSoundPlayer))]
    public class PlayerDeathHandler : MonoBehaviour, IPlayerInitializableComponent
    {
        [SerializeField] private AudioClip[] _deathSounds; 

        private Health _health;
        private RagdollController _ragdollController;
        private KnockbackController _knockbackController;
        private NetworkSoundPlayer _soundPlayer;

        public void InitializeLocalPlayer()
        {
            _health = GetComponent<Health>();
            _ragdollController = GetComponentInChildren<RagdollController>();
            _knockbackController = GetComponent<KnockbackController>();
            _soundPlayer = GetComponent<NetworkSoundPlayer>();

            _health.OnDeath += OnPlayerDeath;

            _soundPlayer.AddClips(nameof(_deathSounds), _deathSounds);
        }

        public void InitializeClientPlayer()
        {
            _soundPlayer = GetComponent<NetworkSoundPlayer>();
            _soundPlayer.AddClips(nameof(_deathSounds), _deathSounds);
            Destroy(this);
        }

        private void OnDisable()
        {
            if(_health != null)
                _health.OnDeath -= OnPlayerDeath;            
        }

        private void OnPlayerDeath(ulong clientID)
        {
            Debug.Log($"OnPlayerDeath() invoke client({clientID})");

            _knockbackController.enabled = false;
            _ragdollController.RequestEnableRagdollServerRpc();
            _soundPlayer.PlaySoundServerRpc(nameof(_deathSounds), Random.Range(0, _deathSounds.Length));

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