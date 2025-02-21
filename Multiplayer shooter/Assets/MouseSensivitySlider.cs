using UnityEngine;
using UnityEngine.UI;

namespace FabrShooter 
{
    public class MouseSensivitySlider : MonoBehaviour
    {
        public const string MOUSE_SENSIVITY_STRING = "MouseSensivity";
        private Slider _slider;

        private void Start()
        {
            _slider = GetComponentInChildren<Slider>();

            _slider.onValueChanged.AddListener(SetSensitivityValue);

            float sensivityValue = PlayerPrefs.GetFloat(MOUSE_SENSIVITY_STRING);

            _slider.SetValueWithoutNotify(sensivityValue);
        }

        private void SetSensitivityValue(float value)
        {
            PlayerPrefs.SetFloat(MOUSE_SENSIVITY_STRING, value);
            PlayerPrefs.Save();
        }
    }
}
