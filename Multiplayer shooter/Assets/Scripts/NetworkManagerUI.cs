using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button _hostBtn;
    [SerializeField] private Button _clientBtn;

    public void StartHost()
    {
        try
        {
            NetworkManager.Singleton.StartHost();
        }
        catch
        {
            Application.Quit();
        }
    }

    public void StartClient()
    {
        try
        {
            NetworkManager.Singleton.StartClient();
        }
        catch
        {
            Application.Quit();
        }
    }
}
