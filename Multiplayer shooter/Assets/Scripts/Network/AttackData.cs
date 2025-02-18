namespace FabrShooter
{
    public struct AttackData
    {
        public readonly DamageSenderType SenderType;
        public readonly ulong TargetID;
        public readonly int Damage;
        public readonly bool UseKnockback;

        public AttackData(DamageSenderType senderType, ulong targetID, int damage, bool useKnockback = false)
        {
            SenderType = senderType;
            TargetID = targetID;
            Damage = damage;
            UseKnockback = useKnockback;
        }
    }
}