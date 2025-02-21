using FabrShooter;
using Unity.Netcode;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private GameObject _cameraObject;
    [SerializeField] private CharacterController _characterController;

    private IPlayerInitializableComponent[] _initializableScripts;

    public override void OnNetworkSpawn()
    {
        _initializableScripts = GetComponentsInChildren<IPlayerInitializableComponent>();
        if (IsOwner)
        {
            foreach(IPlayerInitializableComponent component in _initializableScripts)
            {
                component.InitializeLocalPlayer();
            }

            gameObject.name = "Local Player";
        }
        else
        {
            foreach (IPlayerInitializableComponent component in _initializableScripts)
            {
                component.InitializeClientPlayer();
            }

            gameObject.name = "Client Player";
        }
    }
}
