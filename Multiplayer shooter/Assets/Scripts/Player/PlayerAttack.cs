using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : NetworkBehaviour
{
    [SerializeField] private int _damage;

    private Transform _cameraTransform;
    private PlayerInput _playerInput;

    private void Start()
    {
        if (!IsOwner)
        {
            Destroy(this);
            return;
        }

        _cameraTransform = GetComponentInChildren<Camera>().transform;
        _playerInput = new PlayerInput();

        _playerInput.Enable();
        _playerInput.Player.Attack.performed += Attack;
    }

    private void OnDisable()
    {
        _playerInput.Player.Attack.performed -= Attack;
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

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
        {
            health.TakeDamageServerRpc(_damage);
        }
    }
}
