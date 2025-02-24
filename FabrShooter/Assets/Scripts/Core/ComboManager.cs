using FabrShooter.Core;
using System;
using System.Collections;
using UnityEngine;

namespace FabrShooter
{
    public class ComboManager : MonoBehaviour, IService
    {
        private const float DECREASE_DELAY_TIME = 5f;

        private ServerDamageDealer _dealer;
        private Coroutine _delayedComboLevelDecreaseRoutine;

        public event Action ComboLevelValueChanged;

        public int ComboLevel { get; private set; }

        #region MONO
        private void Awake()
        {
            ServiceLocator.Register<ComboManager>(this);
        }

        private void Start ()
        {
            _dealer = FindAnyObjectByType<ServerDamageDealer>();

            _dealer.OnDamageDealing += OnDamageDeal;
        }

        private void OnEnable()
        {
            if (_dealer != null)
                _dealer.OnDamageDealing += OnDamageDeal;
        }

        private void OnDisable()
        {
            if(_dealer != null)
                _dealer.OnDamageDealing -= OnDamageDeal;
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<ComboManager>(this);
        }
        #endregion

        private void OnDamageDeal(AttackData data)
        {
            AddComboLevel();
        }

        private void AddComboLevel()
        {
            ComboLevel++;
            if (_delayedComboLevelDecreaseRoutine == null)
                _delayedComboLevelDecreaseRoutine = StartCoroutine(DelayComboLevelDecrease());

            ComboLevelValueChanged?.Invoke();
        }

        private IEnumerator DelayComboLevelDecrease()
        {
            while(ComboLevel > 0)
            {
                yield return new WaitForSeconds(DECREASE_DELAY_TIME);
                ComboLevel--;
                ComboLevelValueChanged?.Invoke();
            }
        }
    }
}