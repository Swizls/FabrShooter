using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FabrShooter.Core.SceneManagment
{
    public class SceneLoader : MonoBehaviour, IService
    {
        [SerializeField] private int _mainLevelIndex;
        [SerializeField] private int _mainMenuIndex;

        public event Action OnMainLevelLoad;
        public event Action OnMainMenuLoad;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void LoadMainLevel()
        {
            SceneManager.LoadScene(_mainLevelIndex);

            OnMainLevelLoad?.Invoke();
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(_mainMenuIndex);
            OnMainMenuLoad?.Invoke();
        }
    }
}
