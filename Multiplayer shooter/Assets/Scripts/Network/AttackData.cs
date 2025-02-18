using Unity.Netcode;

namespace FabrShooter
{
    public struct AttackData : INetworkSerializable
    {
        private DamageSenderType _senderType;
        private ulong _targetID;
        private int _damage;
        private bool _useKnockback;

        public DamageSenderType DamageSenderType => _senderType;
        public ulong TargetID => _targetID;
        public int Damage => _damage;
        public bool UseKnockback => _useKnockback;

        public AttackData(DamageSenderType senderType, ulong targetID, int damage, bool useKnockback = false)
        {
            _senderType = senderType;
            _targetID = targetID;
            _damage = damage;
            _useKnockback = useKnockback;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _senderType);
            serializer.SerializeValue(ref _targetID);
            serializer.SerializeValue(ref _damage);
            serializer.SerializeValue(ref _useKnockback);
        }
    }
}