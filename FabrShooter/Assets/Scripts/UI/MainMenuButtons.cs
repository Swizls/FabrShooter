using FabrShooter.Core;
using UnityEngine;

namespace FabrShooter
{
    public class MainMenuButtons : MonoBehaviour
    {
        [SerializeField] private IPInputField _ipInputField;

        public void OnHostButtonClick()
        {
            _ipInputField.SetIP();

            ServiceLocator.Get<GameConnectionManager>().StartGameAsHost();
        }

        public void OnJoinButtonClick()
        {
            _ipInputField.SetIP();

            ServiceLocator.Get<GameConnectionManager>().StartGameAsClient();
        }

        public void OnSigleplayerButtonClick()
        {
            ServiceLocator.Get<GameConnectionManager>().StartSingleplayer();
        }
    }
}