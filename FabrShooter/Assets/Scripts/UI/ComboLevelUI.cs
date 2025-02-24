using FabrShooter.Core;
using TMPro;
using UnityEngine;

namespace FabrShooter.UI
{
    public class ComboLevelUI : MonoBehaviour
    {
        private const string LOW_COMBO_LEVEL_NAME = "Loh pedalniy";
        private const string MID_COMBO_LEVEL_NAME = "Chelobas";
        private const string HIGH_COMBO_LEVEL_NAME = "Fabro God";

        private const int MID_COMBO_TRESHOLD = 5;
        private const int HIGH_COMBO_TRESHOLD = 10;

        [SerializeField] private TextMeshProUGUI _value;

        private void Start()
        {
            ServiceLocator.Get<ComboManager>().ComboLevelValueChanged += SetValue;
            SetValue();
        }

        private void OnDestroy()
        {
            ServiceLocator.Get<ComboManager>().ComboLevelValueChanged -= SetValue;
        }

        private void SetValue()
        {
            int comboLevel = ServiceLocator.Get<ComboManager>().ComboLevel;

            if (comboLevel < MID_COMBO_TRESHOLD)
                _value.text = LOW_COMBO_LEVEL_NAME;
            else if (comboLevel < HIGH_COMBO_TRESHOLD)
                _value.text = MID_COMBO_LEVEL_NAME;
            else
                _value.text = HIGH_COMBO_LEVEL_NAME;

        }
    }
}