using FabrShooter.Core.SceneManagment;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FabrShooter.Core
{
    public class GameConnectionManager : IService
    {
        [SerializeField] private SceneLoader _sceneLoader;

        public event Action OnGameStart;
        public event Action OnGameStop;

        public GameConnectionManager(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void StartGameAsHost()
        {
            _sceneLoader.LoadMainLevel();
            SceneManager.sceneLoaded += OnSceneLoadedAsHost;
        }

        public void StartGameAsClient()
        {
            _sceneLoader.LoadMainLevel();
            SceneManager.sceneLoaded += OnSceneLoadedAsClient;
        }

        private void OnSceneLoadedAsHost(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoadedAsHost;
            NetworkManager.Singleton.StartHost();
            OnGameStart?.Invoke();
            Debug.Log("Starting host");
        }

        private void OnSceneLoadedAsClient(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoadedAsClient;
            NetworkManager.Singleton.StartClient();
            OnGameStart?.Invoke();
            Debug.Log("Starting client");
        }

        public void EndGame()
        {
            Debug.Log("Disconnecting");

            _sceneLoader.LoadMainMenu();
            Cursor.lockState = CursorLockMode.None;
            NetworkManager.Singleton.Shutdown();
            OnGameStop?.Invoke();
        }
    }
}