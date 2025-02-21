using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace FabrShooter
{
    public class HitboxController : NetworkBehaviour
    {
        private List<Hitbox> _hitboxes = new List<Hitbox>();

        public Hitbox LastHittedHitbox { get; private set; }

        public override void OnNetworkSpawn()
        {
            _hitboxes = GetComponentsInChildren<Hitbox>().ToList();
        }

        [ClientRpc]
        public void RegisterHitClientRpc(ulong targetID, ulong hitboxID)
        {
            Hitbox hittedHitbox = _hitboxes.Where(component => component.NetworkBehaviourId == hitboxID).First();

            LastHittedHitbox = hittedHitbox;
        }
    }
}