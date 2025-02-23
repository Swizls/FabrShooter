using System.Collections;
using System.Threading.Tasks;
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
        private HitboxHitHandler _hitboxController;

        public override void OnNetworkSpawn()
        {
            _ragdollController = GetComponent<RagdollController>();
            _soundPlayer = GetComponent<NetworkSoundPlayer>();
            _hitboxController = GetComponentInChildren<HitboxHitHandler>();

            _soundPlayer.AddClips(nameof(_knockbackSounds), _knockbackSounds);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        [ClientRpc]
        public void ApplyKnockbackClientRpc(float knockbackForce)
        {
            if (_ragdollController.IsRagdollActive) return;

            _soundPlayer.PlaySoundServerRpc(nameof(_knockbackSounds), Random.Range(0, _knockbackSounds.Length));
            _ragdollController.RequestEnableRagdollServerRpc();

            StartCoroutine(WaitForKnockbackEnd(KNOCKBACK_TIME, knockbackForce));
        }
        private IEnumerator WaitForKnockbackEnd(float time, float knockbackForce)
        {
            float timer = time;

            yield return new WaitUntil(() => _ragdollController.IsRagdollActive);

            _hitboxController.LastHittedHitbox.Rigidbody.AddForce(Vector3.up * knockbackForce, ForceMode.Impulse);
            Debug.Log($"Knockback force is applied to {_hitboxController.LastHittedHitbox.name} hitbox; Recieved force: {knockbackForce}");

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            if (enabled == false)
                yield return null;

            _ragdollController.RequestDisableRagdollServerRpc();
        }
    }
}