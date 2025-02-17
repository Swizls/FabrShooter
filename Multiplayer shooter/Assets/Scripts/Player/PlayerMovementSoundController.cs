using Game.Input;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerMovementSoundController : NetworkBehaviour
{
    [SerializeField] private AudioClip[] _walkingSFX;
    [SerializeField] private AudioClip[] _runningSFX;
    [SerializeField] private AudioClip[] _jumpingSFX;

    private AudioSource _audioSource;
    private PlayerMovement _playerMovement;

    public override void OnNetworkSpawn()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerMovement = GetComponentInParent<PlayerMovement>();

        _playerMovement.Jumped += PlayJumpSound;
    }

    private void OnDisable()
    {
        _playerMovement.Jumped -= PlayJumpSound;
    }

    private void Update()
    {
        if (!IsOwner) return; 

        if (_playerMovement.IsMoving && !_audioSource.isPlaying)
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
        _audioSource.clip = _runningSFX[Random.Range(0, _runningSFX.Length)];
        _audioSource.Play();

        PlayRunningSoundClientRpc();
    }

    private void PlayJumpSound()
    {
        _audioSource.clip = _jumpingSFX[Random.Range(0, _jumpingSFX.Length)];
        _audioSource.Play();

        PlayJumpSoundClientRpc();
    }

    private void PlayWalkingSound()
    {
        _audioSource.clip = _walkingSFX[Random.Range(0, _walkingSFX.Length)];
        _audioSource.Play();

        PlayFootstepSoundClientRpc();
    }

    private void StopAnySound()
    {
        _audioSource.Stop();

        StopAnySoundClientRpc();
    }

    [ClientRpc]
    private void PlayJumpSoundClientRpc()
    {
        if (IsOwner) return;

        _audioSource.clip = _jumpingSFX[Random.Range(0, _jumpingSFX.Length)];
        _audioSource.Play();
    }

    [ClientRpc]
    private void PlayFootstepSoundClientRpc()
    {
        if(IsOwner) return;

        _audioSource.clip = _walkingSFX[Random.Range(0, _walkingSFX.Length)];
        _audioSource.Play();
    }

    [ClientRpc]
    private void PlayRunningSoundClientRpc()
    {
        _audioSource.clip = _runningSFX[Random.Range(0, _runningSFX.Length)];
        _audioSource.Play();
    }


    [ClientRpc]
    private void StopAnySoundClientRpc()
    {
        if (IsOwner) return;

        _audioSource.Stop();
    }
}