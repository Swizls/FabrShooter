using FabrShooter.Core;
using System;
using System.Threading.Tasks;

namespace FabrShooter
{
    public class ComboManager : IService
    {
        private const float DECREASE_DELAY_TIME = 5f;

        private ServerDamageDealer _dealer;
        private Task _delayedComboLevelDecreaseRoutine;

        public event Action ComboLevelValueChanged;

        public int ComboLevel { get; private set; }

        public ComboManager (ServerDamageDealer dealer)
        {
            _dealer = dealer;

            _dealer.OnDamageDealing += OnDamageDeal;
        }
        private void OnDamageDeal(AttackData data)
        {
            AddComboLevel();
        }

        private void AddComboLevel()
        {
            ComboLevel++;
            if (_delayedComboLevelDecreaseRoutine == null)
                DelayComboLevelDecrease();

            ComboLevelValueChanged?.Invoke();
        }

        private async void DelayComboLevelDecrease()
        {
            while(ComboLevel > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds(DECREASE_DELAY_TIME));
                ComboLevel--;
                ComboLevelValueChanged?.Invoke();
            }
        }
    }
}