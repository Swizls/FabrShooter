using FabrShooter.Core;
using FabrShooter.Core.SceneManagment;
using System;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter
{
    public class GameEntryPoint : MonoBehaviour 
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private NetworkManager _networkManager;

        private GameConnectionManager _connectionManager;

        private bool _isAnotherGameEntryPointInScene;

        public event Action GameInitialized;

        private void Awake()
        {
            _isAnotherGameEntryPointInScene = FindObjectsByType<GameEntryPoint>(FindObjectsSortMode.None).Length > 1;

            if (_isAnotherGameEntryPointInScene)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (_isAnotherGameEntryPointInScene)
                return;

            Instantiate(_networkManager);
            _connectionManager = new GameConnectionManager(_sceneLoader);

            ServiceLocator.Register<GameConnectionManager>(_connectionManager);
            ServiceLocator.Register<SceneLoader>(_sceneLoader);

            GameInitialized?.Invoke();
        }

        private void OnDestroy()
        {
            if (_isAnotherGameEntryPointInScene)
                return;

            ServiceLocator.Clear();
        }
    }
}