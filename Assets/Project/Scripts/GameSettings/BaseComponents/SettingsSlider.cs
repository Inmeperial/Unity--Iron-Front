using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSettings
{
    public abstract class SettingsSlider : SettingItem
    {
        [Header("References")]
        [SerializeField] protected TextMeshProUGUI _statusText;
        [SerializeField] protected Slider _slider;

        protected override void Configure()
        {
            _slider.onValueChanged.AddListener(OnValueChange);
        }

        protected virtual void OnValueChange(float value)
        {
            _statusText.text = (int)value + "%";

            OnSettingChange();
        }

        protected virtual void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnValueChange);
        }
    }
}

