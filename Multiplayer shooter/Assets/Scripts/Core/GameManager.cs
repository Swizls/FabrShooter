using FabrShooter.Core.SceneManagment;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;

        private bool _startGameAsHost;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void StartGame()
        {
            _sceneLoader.LoadMainLevel();

            StartCoroutine(StartMultiplayerGameWithDelay());
        }

        public void EndGame()
        {
            NetworkManager.Singleton.Shutdown();
        }

        public void Disconnect(ulong clientID)
        {
            _sceneLoader.LoadMainMenu();
            Cursor.lockState = CursorLockMode.None;

            NetworkManager.Singleton.OnClientDisconnectCallback -= Disconnect;
        }

        public void SetIsHostOrClient(bool flag)
        {
            _startGameAsHost = flag;
        }

        private IEnumerator StartMultiplayerGameWithDelay()
        {
            yield return new WaitForSeconds(0.1f);

            if (_startGameAsHost)
                NetworkManager.Singleton.StartHost();
            else
                NetworkManager.Singleton.StartClient();

            NetworkManager.Singleton.OnClientDisconnectCallback += Disconnect;
        }
    }
}