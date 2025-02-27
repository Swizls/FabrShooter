using FabrShooter.Player;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FabrShooter.Core
{
    public class GameSessionManager : IService
    {
        public enum SessionType
        {
            Host,
            Client,
            Local
        }

        private SceneDataSO _config;

        public event Action OnGameStart;
        public event Action OnGameStop;

        public SessionType Type { get; private set; }

        public GameSessionManager(SceneDataSO config)
        {
            _config = config;
        }

        public void StartGameAsHost()
        {
            Type = SessionType.Host;
            SceneManager.LoadScene(_config.MainLevelBuildIndex);
            SceneManager.sceneLoaded += OnSceneLoadedAsHost;
        }

        public void StartGameAsClient()
        {
            Type = SessionType.Client;
            SceneManager.LoadScene(_config.MainLevelBuildIndex);
            SceneManager.sceneLoaded += OnSceneLoadedAsClient;
        }

        public void StartSingleplayer()
        {
            Type = SessionType.Local;
            SceneManager.LoadScene(_config.TestLevelBuildIndex);
            SceneManager.sceneLoaded += OnSceneLoadedInSingleplayer;
        }

        public void EndGame()
        {
            Debug.Log("Disconnecting");

            SceneManager.LoadScene(_config.MainMenuBuildIndex);
            NetworkManager.Singleton.Shutdown();

            OnGameStop?.Invoke();
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

        private void OnSceneLoadedInSingleplayer(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= OnSceneLoadedInSingleplayer;
            GameObject.FindAnyObjectByType<PlayerInitilaizer>().InitializeSingleplayerMode();
        }
    }
}