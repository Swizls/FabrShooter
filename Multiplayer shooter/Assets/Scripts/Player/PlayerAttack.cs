using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (AudioSource))]
public class PlayerAttack : NetworkBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private AudioClip _shotSFX;

    private Transform _cameraTransform;
    private PlayerInput _playerInput;
    private AudioSource _audioSource;

    public override void OnNetworkSpawn()
    {
        _cameraTransform = GetComponentInChildren<Camera>().transform;
        _audioSource = GetComponent<AudioSource>();

        _playerInput = new PlayerInput();

        _playerInput.Enable();
        _playerInput.Player.Attack.performed += Attack;
    }

    private void OnDisable()
    {
        if(_playerInput != null )
            _playerInput.Player.Attack.performed -= Attack;
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        PlayShotSFX();
        Debug.Log("Client: " + OwnerClientId + "; Made shot;");

        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit))
        {
            if (hit.collider.gameObject.TryGetComponent(out NetworkObject networkObject))
            {
                ulong targetId = networkObject.NetworkObjectId;
                AttackServerRpc(targetId);
            }
        }
    }

    [ServerRpc]
    private void AttackServerRpc(ulong targetId)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(targetId, out NetworkObject targetObject))
            return;

        if (targetObject.TryGetComponent(out Health health))
            health.TakeDamageServerRpc(_damage);
    }

    private void PlayShotSFX()
    {
        _audioSource.clip = _shotSFX;
        _audioSource.Play();

        PlayShotSFXClientRpc();
    }

    [ClientRpc]
    private void PlayShotSFXClientRpc()
    {
        _audioSource.clip = _shotSFX;
        _audioSource.Play();
    }
}
