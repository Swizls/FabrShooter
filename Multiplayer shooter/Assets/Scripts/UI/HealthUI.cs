using TMPro;
using UnityEngine;

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
    }

    private void OnDisable()
    {
        if(_health != null)
            _health.OnValueChange -= OnValueChange;
    }

    private void OnValueChange()
    {
        _healthValueText.text = _health.Value.ToString();
    }
}
