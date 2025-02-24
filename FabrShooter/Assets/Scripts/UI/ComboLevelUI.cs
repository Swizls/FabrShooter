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

        [SerializeField] private TextMeshProUGUI _value;

        private void Start()
        {
            ServiceLocator.Get<ComboManager>().ComboStateChanged += SetValue;
            SetValue();
        }

        private void OnDestroy()
        {
            ServiceLocator.Get<ComboManager>().ComboStateChanged -= SetValue;
        }

        private void SetValue()
        {
            switch(ServiceLocator.Get<ComboManager>().ComboSate)
            {
                case ComboManager.ComboState.Low:
                    _value.text = LOW_COMBO_LEVEL_NAME;
                    break;
                case ComboManager.ComboState.Mid:
                    _value.text = MID_COMBO_LEVEL_NAME;
                    break;
                case ComboManager.ComboState.High:
                    _value.text = HIGH_COMBO_LEVEL_NAME;
                    break;
            }
        }
    }
}