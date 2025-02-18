using Unity.Netcode;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private GameObject _cameraObject;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private MonoBehaviour[] _localScripts;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) return;

        _cameraObject.SetActive(false);
        _characterController.enabled = false;

        foreach (var script in _localScripts)
        {
            Destroy(script);
        }
    }
}
