using TMPro;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class TestIP : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private UnityTransport _transport;

    private void Awake()
    {
        string ipAddress = _transport.ConnectionData.Address.ToString();
         _inputField.text = ipAddress;
        Debug.Log(_inputField.text);
    }

    public void SetIP()
    {
        Debug.Log(_inputField.text);

        _transport.ConnectionData.Address = _inputField.text;
    }
}
