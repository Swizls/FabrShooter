using UnityEngine;
using UnityEngine.InputSystem;
using FabrShooter.Input;
using System;
using System.Collections;

namespace FabrShooter 
{
    [RequireComponent(typeof(NetworkSoundPlayer))]
    [RequireComponent(typeof(Inventory))]
    public class PlayerAttack : MonoBehaviour, IPlayerInitializableComponent
    {
        private const float MAX_ATTACK_RANGE = 5f;
        private const float PUNCH_COOLDOWN_TIME = 2f;

        [SerializeField] private AudioClip[] _shotSFX;

        private Transform _cameraTransform;

        private PlayerInputActions _playerInputActions;
        private Inventory _inventory;
        private NetworkSoundPlayer _soundPlayer;

        private ServerDamageDelaer _damageDealer;

        private bool _isPunchOnCooldown = false;

        public event Action OnPunch;

        public void InitializeLocalPlayer()
        {
            _cameraTransform = GetComponentInChildren<Camera>().transform;
            _soundPlayer = GetComponent<NetworkSoundPlayer>();
            _inventory = GetComponent<Inventory>();

            _playerInputActions = new PlayerInputActions();

            _playerInputActions.Enable();
            _playerInputActions.Player.Attack.performed += WeaponAttack;
            _playerInputActions.Player.Punch.performed += Punch;

            _damageDealer = FindAnyObjectByType<ServerDamageDelaer>();

            _soundPlayer.AddClips(nameof(_shotSFX), _shotSFX);
        }

        public void InitializeClientPlayer()
        {
            _soundPlayer = GetComponent<NetworkSoundPlayer>();

            _soundPlayer.AddClips(nameof(_shotSFX), _shotSFX);
            Destroy(this);
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
                if (hit.collider.gameObject.TryGetComponent(out Hitbox hitbox))
                {

                    AttackData attackData = new AttackData(
                        DamageSenderType.Client,
                        hitbox.NetworkBehaviourId,
                        hitbox.NetworkObjectId,
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
            StartCoroutine(StartPunchCooldown());

            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit))
            {
                if (Vector3.Distance(transform.position, hit.point) > MAX_ATTACK_RANGE)
                    return;

                Debug.Log($"Client perform attack; Hitted {hit.collider.gameObject.name}");
                if (hit.collider.gameObject.TryGetComponent(out Hitbox hitbox))
                {
                    AttackData attackData = new AttackData(
                        DamageSenderType.Client,
                        hitbox.NetworkBehaviourId,
                        hitbox.NetworkObjectId,
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
            _soundPlayer.PlaySoundServerRpc(nameof(_shotSFX), UnityEngine.Random.Range(0, _shotSFX.Length));
        }

        private IEnumerator StartPunchCooldown()
        {
            yield return new WaitForSeconds(PUNCH_COOLDOWN_TIME);
            _isPunchOnCooldown = false;
        }
    }
}