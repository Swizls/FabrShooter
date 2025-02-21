using FabrShooter.Core;
using FabrShooter.Core.SceneManagment;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter 
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform[] _spawnPoints;

        public override void OnNetworkSpawn()
        {
            if (!IsServer) return;
                
            NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsServer) return;

            NetworkManager.Singleton.OnClientConnectedCallback -= SpawnPlayer;
        }

        private void SpawnPlayer(ulong clientId)
        {
            Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            GameObject playerInstance = Instantiate(_playerPrefab, spawnPoint.position, spawnPoint.rotation);

            NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
            networkObject.SpawnAsPlayerObject(clientId);
        }

        [ServerRpc(RequireOwnership = false)]
        public void RespawnPlayerServerRpc(ulong clientId)
        {
            if (!IsOwner) return;

            SpawnPlayer(clientId);
        }
    }
}