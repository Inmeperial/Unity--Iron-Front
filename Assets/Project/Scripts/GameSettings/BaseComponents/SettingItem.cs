using System.Threading.Tasks;
using UnityEngine;

namespace GameSettings
{
    public abstract class SettingItem : MonoBehaviour
    {
        public virtual void Initialize()
        {
            Configure();
            Settings.Instance.OnConfigureSettingsRequest += Configure;
            Settings.Instance.OnApplySettingRequest += ApplySetting;
        }
        protected abstract void Configure();
        protected abstract void ApplySetting();

        protected virtual void OnDestroy()
        {
            Settings.Instance.OnConfigureSettingsRequest -= Configure;
            Settings.Instance.OnApplySettingRequest -= ApplySetting;
        }
    }
}

