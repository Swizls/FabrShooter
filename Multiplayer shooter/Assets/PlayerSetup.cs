using Unity.Netcode;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private GameObject _cameraObject;
    [SerializeField] private MonoBehaviour[] _localScripts;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            _cameraObject.SetActive(false);
            foreach (var script in _localScripts)
            {
                script.enabled = false;
            }
        }
    }
}
