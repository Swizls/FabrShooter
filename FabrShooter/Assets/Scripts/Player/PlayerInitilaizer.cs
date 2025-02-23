using Unity.Netcode;
using UnityEngine;

namespace FabrShooter.Player
{
    public class PlayerInitilaizer : NetworkBehaviour
    {
        [SerializeField] private GameObject _cameraObject;
        [SerializeField] private CharacterController _characterController;

        private IPlayerInitializableComponent[] _initializableScripts;

        public void InitializeSingleplayerMode()
        {
            _initializableScripts = GetComponentsInChildren<IPlayerInitializableComponent>();
            InitializeLocalPlayer();
        }

        public override void OnNetworkSpawn()
        {
            _initializableScripts = GetComponentsInChildren<IPlayerInitializableComponent>();
            if (IsOwner)
            {
                InitializeLocalPlayer();
            }
            else
            {
                InitializeClientPlayer();
            }
        }

        private void InitializeClientPlayer()
        {
            foreach (IPlayerInitializableComponent component in _initializableScripts)
            {
                component.InitializeClientPlayer();
            }

            gameObject.name = "Client Player";
        }

        private void InitializeLocalPlayer()
        {
            foreach (IPlayerInitializableComponent component in _initializableScripts)
            {
                component.InitializeLocalPlayer();
            }

            gameObject.name = "Local Player";
        }
    }
}
