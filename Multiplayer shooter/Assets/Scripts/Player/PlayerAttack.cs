using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FabrShooter 
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Inventory))]
    public class PlayerAttack : MonoBehaviour, IPlayerInitializableComponent
    {
        [SerializeField] private int _damage;
        [SerializeField] private AudioClip _shotSFX;

        private Transform _cameraTransform;

        private PlayerInput _playerInput;
        private Inventory _inventory;
        private AudioSource _audioSource;

        private ServerDamageDelaer _damageDealer;

        public void Initialize()
        {
            _cameraTransform = GetComponentInChildren<Camera>().transform;
            _audioSource = GetComponent<AudioSource>();
            _inventory = GetComponent<Inventory>();

            _playerInput = new PlayerInput();

            _playerInput.Enable();
            _playerInput.Player.Attack.performed += Attack;

            _damageDealer = FindAnyObjectByType<ServerDamageDelaer>();
        }

        private void OnEnable()
        {
            if (_playerInput != null)
                _playerInput.Player.Attack.performed += Attack;
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
                if (TryGetNetworkObject(hit.collider.gameObject, out NetworkObject networkObject))
                {
                    ulong targetId = networkObject.NetworkObjectId;

                    AttackData attackData = new AttackData(
                        DamageSenderType.Client,
                        targetId,
                        _inventory.CurrentWeapon.Damage,
                        _inventory.CurrentWeapon.UseKnockback,
                        _inventory.CurrentWeapon.KnockbackForce
                    );

                    _damageDealer.DealDamageServerRpc(attackData);
                }
            }

            bool TryGetNetworkObject(GameObject gameObj, out NetworkObject obj)
            {
                obj = gameObj.GetComponent<NetworkObject>();
                if (obj != null)
                    return true;

                obj = gameObj.GetComponentInChildren<NetworkObject>();
                if (obj != null)
                    return true;

                obj = gameObj.GetComponentInParent<NetworkObject>();
                if (obj != null)
                    return true;

                return false;
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