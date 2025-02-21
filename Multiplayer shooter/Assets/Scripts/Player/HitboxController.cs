using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace FabrShooter
{
    public class HitboxController : NetworkBehaviour
    {
        private Hitbox _lastHittedHitbox;

        public Hitbox LastHittedHitbox => _lastHittedHitbox;

        [ClientRpc]
        public void RegisterHitClientRpc(ulong hitboxID)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(hitboxID, out NetworkObject hitboxObject))
                return;

            var test = hitboxObject.GetComponentsInChildren<Hitbox>().ToList().Where(component => component.NetworkBehaviourId == hitboxID);

            Debug.Log(test.ToList());

            _lastHittedHitbox = test.First();
            Debug.Log($"Last hitted hitbox: {_lastHittedHitbox.name}");
        }
    }
}