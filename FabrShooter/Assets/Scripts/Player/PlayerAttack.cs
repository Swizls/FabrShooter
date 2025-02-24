using UnityEngine;
using UnityEngine.InputSystem;
using FabrShooter.Input;
using System;
using System.Collections;
using FabrShooter.Player.Movement;
using Unity.Netcode;

namespace FabrShooter 
{
    [RequireComponent(typeof(NetworkSoundPlayer))]
    [RequireComponent(typeof(NetworkObject))]
    public class PlayerAttack : MonoBehaviour, IPlayerInitializableComponent
    {
        private const float PUNCH_MAX_ATTACK_RANGE = 5f;
        private const float PUNCH_COOLDOWN_TIME = 2f;

        [SerializeField] private WeaponSO _gunConfig;
        [SerializeField] private WeaponSO _fists;
        [SerializeField] private WeaponSO _slideKick;

        private Transform _cameraTransform;

        private PlayerInputActions _playerInputActions;
        private PlayerMovement _playerMovement;
        private NetworkSoundPlayer _soundPlayer;
        private ServerDamageDealer _damageDealer;

        private ulong _ownerClientID;
        private bool _isPunchOnCooldown = false;
        private bool _isSlideKickActive;

        public event Action OnPunch;

        #region MONO
        public void InitializeLocalPlayer()
        {
            _cameraTransform = GetComponentInChildren<Camera>().transform;
            _soundPlayer = GetComponent<NetworkSoundPlayer>();
            _playerMovement = GetComponent<PlayerMovement>();
            _ownerClientID = GetComponent<NetworkObject>().OwnerClientId;

            _playerMovement.SlideStateChanged += (bool value) => _isSlideKickActive = value;

            _damageDealer = FindAnyObjectByType<ServerDamageDealer>();

            _soundPlayer.AddClips(nameof(_gunConfig), _gunConfig.SFX);
            _soundPlayer.AddClips(nameof(_fists), _fists.SFX);
            _soundPlayer.AddClips(nameof(_slideKick), _slideKick.SFX);
        }

        public void InitializeClientPlayer()
        {
            _soundPlayer = GetComponent<NetworkSoundPlayer>();

            _soundPlayer.AddClips(nameof(_gunConfig), _gunConfig.SFX);
            _soundPlayer.AddClips(nameof(_fists), _fists.SFX);
            _soundPlayer.AddClips(nameof(_slideKick), _slideKick.SFX);
            Destroy(this);
        }

        private void OnEnable()
        {
            if (_playerInputActions == null)
                _playerInputActions = new PlayerInputActions();

            _playerInputActions.Player.Enable();
            _playerInputActions.Player.Attack.performed += WeaponAttack;
            _playerInputActions.Player.Punch.performed += Punch;

            if(_playerMovement != null)
                _playerMovement.SlideStateChanged -= (bool value) => _isSlideKickActive = value;
        }

        private void OnDisable()
        {
            if (_playerInputActions == null)
                return;

            _playerInputActions.Player.Attack.performed -= WeaponAttack;
            _playerInputActions.Player.Punch.performed -= Punch;
            _playerInputActions.Player.Disable();


            if (_playerMovement != null)
                _playerMovement.SlideStateChanged -= (bool value) => _isSlideKickActive = value;
        }

        private void Update()
        {
            if (!_isSlideKickActive)
                return;

            SlideKickAttack();
        }

        private void SlideKickAttack()
        {
            Vector3 position = transform.position + transform.forward;

            if (!Physics.SphereCast(position, 2f, transform.forward, out RaycastHit hit, 2f, LayerMask.GetMask("Player")))
                return;

            if (!hit.collider.TryGetComponent(out Hitbox hitbox))
                return;

            if (hitbox.HitboxController.RagdollController.IsRagdollActive)
                return;

            _soundPlayer.PlaySoundServerRpc(nameof(_slideKick), UnityEngine.Random.Range(0, _slideKick.SFX.Length));
            _damageDealer.DealDamageServerRpc(CreateAttackData(hitbox, _slideKick));
        }

        #endregion

        private void WeaponAttack(InputAction.CallbackContext context)
        {
            _soundPlayer.PlaySoundServerRpc(nameof(_gunConfig), UnityEngine.Random.Range(0, _gunConfig.SFX.Length));

            if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit))
                return;

            if (!hit.collider.gameObject.TryGetComponent(out Hitbox hitbox))
                return;

            _damageDealer.DealDamageServerRpc(CreateAttackData(hitbox, _gunConfig));
        }

        private void Punch(InputAction.CallbackContext context)
        {
            if (_isPunchOnCooldown)
                return;

            OnPunch?.Invoke();

            _isPunchOnCooldown = true;
            StartCoroutine(StartPunchCooldown());

            if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit))
                return;

            if (Vector3.Distance(transform.position, hit.point) > PUNCH_MAX_ATTACK_RANGE)
                return;

            if (!hit.collider.gameObject.TryGetComponent(out Hitbox hitbox))
                return;

            _soundPlayer.PlaySoundServerRpc(nameof(_fists), UnityEngine.Random.Range(0, _fists.SFX.Length));
            _damageDealer.DealDamageServerRpc(CreateAttackData(hitbox, _fists));
        }

        private AttackData CreateAttackData(Hitbox hitbox, WeaponSO config)
        {
            return new AttackData(
                        DamageSenderType.Client,
                        _ownerClientID,
                        hitbox.NetworkObjectId,
                        hitbox.NetworkBehaviourId,
                        config.Damage,
                        config.AttackType, 
                        config.UseKnockback,
                        config.KnockbackForce);
        }

        private IEnumerator StartPunchCooldown()
        {
            yield return new WaitForSeconds(PUNCH_COOLDOWN_TIME);
            _isPunchOnCooldown = false;
        }
    }
}