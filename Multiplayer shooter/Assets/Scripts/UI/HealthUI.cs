using TMPro;
using UnityEngine;

namespace FabrShooter.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthTitleText;
        [SerializeField] private TextMeshProUGUI _healthValueText;

        private Health _health;

        private void Start()
        {
            _healthTitleText.gameObject.SetActive(false);
            _healthValueText.gameObject.SetActive(false);
        }

        public void Initialize(Health health)
        {
            _health = health;
            _health.OnValueChange += OnValueChange;

            _healthTitleText.gameObject.SetActive(true);
            _healthValueText.gameObject.SetActive(true);

            SetValue(_health.Value);
        }

        private void OnDisable()
        {
            if (_health != null)
                _health.OnValueChange -= OnValueChange;
        }

        private void OnValueChange()
        {
            SetValue(_health.Value);
        }

        private void SetValue(int value)
        {
            _healthValueText.text = value.ToString();
        }
    }
}

