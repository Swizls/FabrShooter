using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;

namespace FabrShooter
{
    public class HitboxHitHandler : NetworkBehaviour
    {
        private const string PLAY_EVENT_NAME = "OnPlay";

        [SerializeField] private VisualEffect _bloodVFX;

        private List<Hitbox> _hitboxes = new List<Hitbox>();

        public Hitbox LastHittedHitbox { get; private set; }
        public RagdollController RagdollController { get; private set; }

        public override void OnNetworkSpawn()
        {
            _hitboxes = GetComponentsInChildren<Hitbox>().ToList();
            RagdollController = GetComponentInParent<RagdollController>();

            _bloodVFX.Stop();
        }

        [ClientRpc]
        public void RegisterHitClientRpc(ulong targetID, ulong hitboxID)
        {
            Hitbox hittedHitbox = _hitboxes.Where(component => component.NetworkBehaviourId == hitboxID).First();

            LastHittedHitbox = hittedHitbox;

            _bloodVFX.SendEvent(PLAY_EVENT_NAME);
        }
    }
}