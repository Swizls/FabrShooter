using FabrShooter.UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FabrShooter.Core
{
    public class LevelEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private ServerDamageDealer _damageDealer;
        [SerializeField] private LevelMusicController _musicController;

        [Space]
        [Header("UI")]
        [SerializeField] private ComboLevelUI _comboLevelUI;
        [SerializeField] private HealthUI _healthUI;

        private ComboManager _comboManager;

        private void Awake()
        {
            try
            {
                ServiceLocator.Get<GameSessionManager>();
            }
            catch
            {
                SceneManager.LoadScene(Resources.Load<SceneDataSO>("Scene Data").MainMenuBuildIndex);
            }

            _comboManager = new ComboManager(_damageDealer);
            _playerSpawner.PlayerSpawned += OnPlayerSpawn;
        }

        private void OnPlayerSpawn(ulong networkObjectID)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectID, out NetworkObject networkObject))
                return;

            _healthUI.Initialize(networkObject.GetComponent<Health>());
        }

        private void Start()
        {
            _comboLevelUI.Initialize(_comboManager);
            _musicController.Initialize(_comboManager);
        }
    }
}