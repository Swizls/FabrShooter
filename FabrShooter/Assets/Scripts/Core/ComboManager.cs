using FabrShooter.Core;
using System;
using System.Threading.Tasks;
using Unity.Netcode;

namespace FabrShooter
{
    public class ComboManager : IService
    {
        public enum ComboState 
        { 
            Low,
            Mid,
            High
        }

        private const float DECREASE_DELAY_TIME = 5f;

        private const int MID_COMBO_TRESHOLD = 5;
        private const int HIGH_COMBO_TRESHOLD = 10;

        private ServerDamageDealer _dealer;
        private Task _delayedComboLevelDecreaseTask;

        public event Action ComboStateChanged;

        public int ComboValue { get; private set; }
        public ComboState ComboSate { get; private set; }

        public ComboManager (ServerDamageDealer dealer)
        {
            _dealer = dealer;

            _dealer.OnDamageDealing += OnDamageDeal;
        }
        private void OnDamageDeal(AttackData data)
        {
            if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(data.TargetID, out NetworkObject netObject))
                return;

            Hitbox lastHittedHitbox = netObject.GetComponentInChildren<HitboxHitHandler>().LastHittedHitbox;

            switch (lastHittedHitbox.Hitboxtype)
            {
                case Hitbox.HitboxType.Head:
                    AddComboLevel(2);
                    break;
                case Hitbox.HitboxType.Balls:
                    AddComboLevel(3);
                    break;
            }

            if (data.Attacktype == AttackData.AttackType.Punch)
                AddComboLevel(1);

            if (data.Attacktype == AttackData.AttackType.SlideKick)
                AddComboLevel(1);
        }

        private void AddComboLevel(int value)
        {
            ComboValue += value;

            if (_delayedComboLevelDecreaseTask == null)
                _delayedComboLevelDecreaseTask = DelayComboLevelDecrease();

            ChangeComboState();
        }

        private void ChangeComboState()
        {
            ComboState previousState = ComboSate;

            if (ComboValue < MID_COMBO_TRESHOLD)
                ComboSate = ComboState.Low;
            else if (ComboValue < HIGH_COMBO_TRESHOLD)
                ComboSate = ComboState.Mid;
            else
                ComboSate = ComboState.High;

            if (previousState == ComboSate)
                return;

            ComboStateChanged?.Invoke();
        }

        private async Task DelayComboLevelDecrease()
        {
            while(ComboValue > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(DECREASE_DELAY_TIME));
                ComboValue--;
                ChangeComboState();
            }

            _delayedComboLevelDecreaseTask = null;
        }
    }
}