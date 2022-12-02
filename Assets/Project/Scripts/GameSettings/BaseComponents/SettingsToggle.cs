using UnityEngine;
using UnityEngine.UI;

namespace GameSettings
{
    public abstract class SettingsToggle : SettingItem
    {
        [Header("References")]
        [SerializeField] protected Toggle _toggle;

        protected override void Configure()
        {
            _toggle.onValueChanged.AddListener(OnValueChange);
        }

        protected abstract void OnValueChange(bool status);

        protected virtual void OnDestroy()
        {
            _toggle.onValueChanged.RemoveListener(OnValueChange);
        }
    }
}

