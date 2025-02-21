using Unity.Netcode;

namespace FabrShooter
{
    public struct AttackData : INetworkSerializable
    {
        private DamageSenderType _senderType;
        private ulong _targetID;
        private ulong _hitboxID;
        private int _damage;
        private bool _useKnockback;
        private float _knockbackForce;

        public DamageSenderType DamageSenderType => _senderType;
        public ulong HitboxID => _hitboxID;
        public ulong TargetID => _targetID;
        public int Damage => _damage;
        public bool UseKnockback => _useKnockback;
        public float KnockbackForce => _knockbackForce;

        public AttackData(DamageSenderType senderType, ulong hitboxID, ulong targetID, int damage, bool useKnockback, float knockbackForce)
        {
            _senderType = senderType;
            _hitboxID = hitboxID;
            _targetID = targetID;
            _damage = damage;
            _useKnockback = useKnockback;
            _knockbackForce = knockbackForce;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _senderType);
            serializer.SerializeValue(ref _hitboxID);
            serializer.SerializeValue(ref _targetID);
            serializer.SerializeValue(ref _damage);
            serializer.SerializeValue(ref _useKnockback);
            serializer.SerializeValue(ref _knockbackForce);
        }
    }
}