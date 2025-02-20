using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using FabrShooter.Input;
using System;
using System.Collections;

namespace FabrShooter 
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Inventory))]
    public class PlayerAttack : MonoBehaviour, IPlayerInitializableComponent
    {
        private const float MAX_ATTACK_RANGE = 5f;
        private const float PUNCH_COOLDOWN_TIME = 2f;

        [SerializeField] private int _damage;
        [SerializeField] private AudioClip _shotSFX;

        private Transform _cameraTransform;

        private PlayerInputActions _playerInputActions;
        private Inventory _inventory;
        private AudioSource _audioSource;

        private ServerDamageDelaer _damageDealer;

        private bool _isPunchOnCooldown = false;

        public event Action OnPunch;

        public void Initialize()
        {
            _cameraTransform = GetComponentInChildren<Camera>().transform;
            _audioSource = GetComponent<AudioSource>();
            _inventory = GetComponent<Inventory>();

            _playerInputActions = new PlayerInputActions();

            _playerInputActions.Enable();
            _playerInputActions.Player.Attack.performed += WeaponAttack;
            _playerInputActions.Player.Punch.performed += Punch;

            _damageDealer = FindAnyObjectByType<ServerDamageDelaer>();
        }

        private void OnEnable()
        {
            if (_playerInputActions == null)
                return;

            _playerInputActions.Player.Enable();
            _playerInputActions.Player.Attack.performed += WeaponAttack;
            _playerInputActions.Player.Punch.performed += Punch;
        }

        private void OnDisable()
        {
            if (_playerInputActions == null)
                return;

            _playerInputActions.Player.Disable();
            _playerInputActions.Player.Attack.performed -= WeaponAttack;
            _playerInputActions.Player.Punch.performed -= Punch;
        }

        private void WeaponAttack(InputAction.CallbackContext context)
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
        }

        private void Punch(InputAction.CallbackContext context)
        {
            if (_isPunchOnCooldown)
                return;

            OnPunch?.Invoke();

            _isPunchOnCooldown = true;
            StartCoroutine(PunchCooldown());

            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit))
            {
                if (Vector3.Distance(transform.position, hit.point) > MAX_ATTACK_RANGE)
                    return;

                Debug.Log($"Client perform attack; Hitted {hit.collider.gameObject.name}");
                if (TryGetNetworkObject(hit.collider.gameObject, out NetworkObject networkObject))
                {
                    ulong targetId = networkObject.NetworkObjectId;

                    AttackData attackData = new AttackData(
                        DamageSenderType.Client,
                        targetId,
                        5,
                        true,
                        50
                    );

                    _damageDealer.DealDamageServerRpc(attackData);
                }
            }
        }

        private void PlayShotSFX()
        {
            _audioSource.clip = _shotSFX;
            _audioSource.Play();
        }

        private IEnumerator PunchCooldown()
        {
            yield return new WaitForSeconds(PUNCH_COOLDOWN_TIME);
            _isPunchOnCooldown = false;
        }

        private bool TryGetNetworkObject(GameObject gameObj, out NetworkObject obj)
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
}