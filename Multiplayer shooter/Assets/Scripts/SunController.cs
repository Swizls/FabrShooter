using Unity.Netcode;
using UnityEngine;

public class SunController : NetworkBehaviour
{
    [SerializeField] private Light _sun;

    [ClientRpc]
    public void TurnOffSunClientRpc()
    {
        _sun.enabled = false;
    }

    [ClientRpc]
    public void TurnOnSunClientRpc()
    {    
        _sun.enabled = true;
    }
}
