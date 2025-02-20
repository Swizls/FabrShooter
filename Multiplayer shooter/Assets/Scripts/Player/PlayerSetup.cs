using FabrShooter;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private GameObject _cameraObject;
    [SerializeField] private CharacterController _characterController;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            InitializeLocalPlayer();
        else
            InitializeClientPlayer();
    }

    private void InitializeClientPlayer()
    {
        _cameraObject.SetActive(false);
        _characterController.enabled = false;

        DestroyLocalScripts();
    }

    private void InitializeLocalPlayer()
    {
        List<IPlayerInitializableComponent> components = GetComponentsInChildren<IPlayerInitializableComponent>().ToList();

        foreach (IPlayerInitializableComponent component in components)
            component.Initialize();
    }

    private void DestroyLocalScripts()
    {
        List<MonoBehaviour> localScripts = GetComponentsInChildren<MonoBehaviour>()
                                        .Where(component => !typeof(NetworkBehaviour).IsAssignableFrom(component.GetType())
                                        && component.GetType() != typeof(NetworkObject)).ToList();

        foreach (var script in localScripts)
            Destroy(script);
    }
}
