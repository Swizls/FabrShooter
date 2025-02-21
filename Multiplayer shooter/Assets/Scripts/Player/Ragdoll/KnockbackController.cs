using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter
{
    [RequireComponent (typeof(RagdollController))]
    [RequireComponent(typeof(NetworkSoundPlayer))]
    public class KnockbackController : NetworkBehaviour
    {
        private const float KNOCKBACK_TIME = 5f;

        [SerializeField] private Rigidbody _targetBoneRigidbody;
        [SerializeField] private AudioClip[] _knockbackSounds;
        
        private RagdollController _ragdollController;
        private NetworkSoundPlayer _soundPlayer;

        private void Start()
        {
            _ragdollController = GetComponent<RagdollController>();
            _soundPlayer = GetComponent<NetworkSoundPlayer>();

            _soundPlayer.AddClips(nameof(_knockbackSounds), _knockbackSounds);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        [ClientRpc]
        public void ApplyKnockbackClientRpc(float knockbackForce)
        {
            if (!IsOwner) return;

            if (_ragdollController.IsRagdollActive) return;

            _targetBoneRigidbody.AddForce(-transform.forward * knockbackForce, ForceMode.Force);

            StartCoroutine(WaitForKnockbackEnd(KNOCKBACK_TIME));
            _soundPlayer.PlaySoundServerRpc(nameof(_knockbackSounds), Random.Range(0, _knockbackSounds.Length));
            _ragdollController.RequestEnableRagdollServerRpc();
        }

        private IEnumerator WaitForKnockbackEnd(float time)
        {
            float timer = time;

            Debug.Log($"Knockback timer started for client({OwnerClientId})");

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            if (enabled == false)
                yield return null;

            Debug.Log($"Knockback timer ended for client({OwnerClientId}; Requesting ragdoll disabling)");

            _ragdollController.RequestDisableRagdollServerRpc();
        }
    }
}