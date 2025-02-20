using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter 
{
    [RequireComponent(typeof(AudioSource))]
    public class NetworkSoundPlayer : NetworkBehaviour
    {
        private AudioSource _audioSource;

        private Dictionary<string, AudioClip[]> _audioClips = new Dictionary<string, AudioClip[]>();

        public bool IsClipsAdded { get; private set; }
        public bool IsPlaying => _audioSource.isPlaying;

        public override void OnNetworkSpawn()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void AddClips(string key,AudioClip[] audioClipArray)
        {
            _audioClips.Add(key, audioClipArray);
            IsClipsAdded = true;
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlaySoundServerRpc(string key, int clipIndex)
        {
            PlaySoundClientRpc(key, clipIndex);            
        }

        [ServerRpc(RequireOwnership = false)]
        public void StopSoundServerRpc()
        {
            StopSoundClientRpc();
        }

        [ClientRpc]
        private void StopSoundClientRpc()
        {
            _audioSource.Stop();
        }

        [ClientRpc]
        private void PlaySoundClientRpc(string key, int clipIndex)
        {
            _audioSource.PlayOneShot(_audioClips[key][clipIndex]);
        }
    }
}