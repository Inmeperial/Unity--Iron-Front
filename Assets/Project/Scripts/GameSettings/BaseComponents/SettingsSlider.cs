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

        protected abstract void OnValueChange(float value);
    }
}

