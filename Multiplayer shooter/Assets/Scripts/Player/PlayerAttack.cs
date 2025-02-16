using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FabrShooter 
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private AudioClip _shotSFX;

        private Transform _cameraTransform;
        private PlayerInput _playerInput;
        private AudioSource _audioSource;

        private ServerDamageDelaer _damageDealer;

        private void Start()
        {
            _cameraTransform = GetComponentInChildren<Camera>().transform;
            _audioSource = GetComponent<AudioSource>();

            _playerInput = new PlayerInput();

            _playerInput.Enable();
            _playerInput.Player.Attack.performed += Attack;

            _damageDealer = FindAnyObjectByType<ServerDamageDelaer>();
        }

        private void OnDisable()
        {
            if (_playerInput != null)
                _playerInput.Player.Attack.performed -= Attack;
        }

        private void Attack(InputAction.CallbackContext context)
        {
            PlayShotSFX();

            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit))
            {
                Debug.Log($"Client perform attack; Hitted {hit.collider.gameObject.name}");
                if (hit.collider.gameObject.TryGetComponent(out NetworkObject networkObject))
                {
                    ulong targetId = networkObject.NetworkObjectId;
                    _damageDealer.DealDamageServerRpc(targetId, _damage);
                }
            }
        }

        private void PlayShotSFX()
        {
            _audioSource.clip = _shotSFX;
            _audioSource.Play();
        }

        //[ClientRpc]
        //private void PlayShotSFXClientRpc()
        //{
        //    _audioSource.clip = _shotSFX;
        //    _audioSource.Play();
        //}
    }
}