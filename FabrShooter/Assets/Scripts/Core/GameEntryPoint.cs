using FabrShooter.Core;
using System;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter
{
    public class GameEntryPoint : MonoBehaviour 
    {
        [SerializeField] private NetworkManager _networkManagerPrefab;
        [SerializeField] private GameObject _inGameDebugConsolePrefab;

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

            Instantiate(_networkManagerPrefab);
            Instantiate(_inGameDebugConsolePrefab);

            ServiceLocator.Register<GameSessionManager>(new GameSessionManager(Resources.Load<SceneDataSO>("Scene Data")));

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