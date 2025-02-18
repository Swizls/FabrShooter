using FabrShooter;
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
        {
            IPlayerInitializableComponent[] components = GetComponents<IPlayerInitializableComponent>();

            foreach (IPlayerInitializableComponent component in components)
                component.Initialize();
        }
        else
        {

            _cameraObject.SetActive(false);
            _characterController.enabled = false;

            DestroyLocalScripts();
        }
    }

    private void DestroyLocalScripts()
    {
        MonoBehaviour[] localScripts = GetComponents<MonoBehaviour>()
                                        .Where(component => !typeof(NetworkBehaviour).IsAssignableFrom(component.GetType())
                                        && component.GetType() != typeof(NetworkObject))
                                        .ToArray();

        foreach (var script in localScripts)
        {
            Destroy(script);
        }
    }
}
