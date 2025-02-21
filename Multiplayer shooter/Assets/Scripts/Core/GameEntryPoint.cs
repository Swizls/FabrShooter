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

        private bool _isAnotherGEPsInScene;

        public event Action GameInitialized;

        private void Awake()
        {
            //GEP = GameEntryPoint
            _isAnotherGEPsInScene = FindObjectsByType<GameEntryPoint>(FindObjectsSortMode.None).Length > 1;

            if (_isAnotherGEPsInScene)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (_isAnotherGEPsInScene)
                return;

            Instantiate(_networkManager);
            _connectionManager = new GameConnectionManager(_sceneLoader);

            ServiceLocator.Register<GameConnectionManager>(_connectionManager);
            ServiceLocator.Register<SceneLoader>(_sceneLoader);

            GameInitialized?.Invoke();
        }

        private void OnDestroy()
        {
            if (_isAnotherGEPsInScene)
                return;

            ServiceLocator.Clear();
        }
    }
}