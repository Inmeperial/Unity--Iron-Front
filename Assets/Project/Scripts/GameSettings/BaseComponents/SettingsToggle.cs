using UnityEngine;
using UnityEngine.UI;

namespace GameSettings
{
    public abstract class SettingsToggle : SettingItem
    {
        [Header("References")]
        [SerializeField] protected Toggle _toggle;

        public override void OnSettingsLoaded()
        {
            _toggle.onValueChanged.AddListener((bool isActive) => OnValueChange(isActive));
        }

        protected abstract void OnValueChange(bool status);

        protected void OnDestroy()
        {
            _toggle.onValueChanged = null;
        }
    }
}

