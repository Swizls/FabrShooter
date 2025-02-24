using FabrShooter.Core;
using System;
using System.Threading.Tasks;

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
            AddComboLevel();
        }

        private void AddComboLevel()
        {
            ComboValue++;

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