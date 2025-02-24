using UnityEngine;

namespace FabrShooter 
{
    [RequireComponent(typeof(BoxCollider))]
    public class Killbox : MonoBehaviour
    {
        private const int DAMAGE = 1000000;

        private ServerDamageDealer _dealer;
        private BoxCollider _collider;

        private void Start()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;

            _collider.includeLayers = LayerMask.GetMask("Player"); 

            _dealer = FindAnyObjectByType<ServerDamageDealer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Hitbox hitbox = other.GetComponentInChildren<Hitbox>();

            if (hitbox == null)
                return;

            AttackData attackData = new AttackData(
                DamageSenderType.MonoBehaviour, 
                0, 
                hitbox.NetworkObjectId, 
                hitbox.NetworkBehaviourId, 
                DAMAGE, 
                AttackData.AttackType.System);

            _dealer.DealDamageServerRpc(attackData);
        }
    }
}