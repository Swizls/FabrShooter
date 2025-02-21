using FabrShooter.Player;
using UnityEngine;

namespace FabrShooter
{
    [RequireComponent(typeof(NetworkSoundPlayer))]
    public class PlayerMovementSoundController : MonoBehaviour, IPlayerInitializableComponent
    {
        [SerializeField] private AudioClip[] _walkingSFX;
        [SerializeField] private AudioClip[] _runningSFX;
        [SerializeField] private AudioClip[] _jumpingSFX;

        private NetworkSoundPlayer _soundPlayer;
        private PlayerMovement _playerMovement;

        public void InitializeLocalPlayer()
        {
            _soundPlayer = GetComponent<NetworkSoundPlayer>();
            _playerMovement = GetComponentInParent<PlayerMovement>();

            _playerMovement.Jumped += PlayJumpSound;

            _soundPlayer.AddClips(nameof(_walkingSFX), _walkingSFX);
            _soundPlayer.AddClips(nameof(_runningSFX), _runningSFX);
            _soundPlayer.AddClips(nameof(_jumpingSFX), _jumpingSFX);
        }

        public void InitializeClientPlayer()
        {
            _soundPlayer = GetComponent<NetworkSoundPlayer>();

            _soundPlayer.AddClips(nameof(_walkingSFX), _walkingSFX);
            _soundPlayer.AddClips(nameof(_runningSFX), _runningSFX);
            _soundPlayer.AddClips(nameof(_jumpingSFX), _jumpingSFX);

            Destroy(this);
        }

        private void OnEnable()
        {
            if (_playerMovement == null)
                return;
            
            _playerMovement.Jumped += PlayJumpSound;
        }

        private void OnDisable()
        {
            if (_playerMovement == null)
                return;

            _playerMovement.Jumped -= PlayJumpSound;
        }

        private void Update()
        {
            if (_playerMovement.IsMoving && !_soundPlayer.IsPlaying)
            {
                if (_playerMovement.IsFlying)
                    return;

                if (_playerMovement.IsRunning)
                    PlayRunningSound();
                else
                    PlayWalkingSound();
            }
            else if (!_playerMovement.IsMoving && !_playerMovement.IsFlying)
            {
                StopAnySound();
            }
        }

        private void PlayRunningSound()
        {
            _soundPlayer.PlaySoundServerRpc(nameof(_runningSFX), Random.Range(0, _runningSFX.Length));
        }

        private void PlayJumpSound()
        {
            _soundPlayer.PlaySoundServerRpc(nameof(_jumpingSFX), Random.Range(0, _jumpingSFX.Length));
        }

        private void PlayWalkingSound()
        {
            _soundPlayer.PlaySoundServerRpc(nameof(_walkingSFX), Random.Range(0, _walkingSFX.Length));
        }

        private void StopAnySound()
        {
            _soundPlayer.StopSoundServerRpc();
        }
    }
}