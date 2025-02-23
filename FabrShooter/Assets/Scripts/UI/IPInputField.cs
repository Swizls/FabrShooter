using FabrShooter;
using System;
using TMPro;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class IPInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;

    private UnityTransport _transport;

    private void Awake()
    {
        FindFirstObjectByType<GameEntryPoint>().GameInitialized += OnGameInitializing;
    }

    private void OnGameInitializing()
    {
        _transport = FindFirstObjectByType<UnityTransport>();
        string ipAddress = _transport.ConnectionData.Address.ToString();
        _inputField.text = ipAddress;
    }

    public void SetIP()
    {
        _transport.ConnectionData.Address = _inputField.text;
    }
}
