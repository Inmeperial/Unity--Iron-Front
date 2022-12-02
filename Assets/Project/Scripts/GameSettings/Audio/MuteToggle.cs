using System.Threading;
using UnityEngine;
using UnityEngine.Audio;

namespace GameSettings.Audio
{
    public class MuteToggle : SettingsToggle
    {
        [SerializeField] private AudioMixer _globalMixer;

        protected override void Configure()
        {
            base.Configure();
            _toggle.isOn = Settings.Instance.SettingsData.mute;
        }

        protected override void OnSettingChange()
        {
            Settings.Instance.SetMute(_toggle.isOn);
        }

        protected override void OnValueChange(bool status)
        {
            Settings.Instance.SettingsData.mute = status;
            OnSettingChange();
        }
    }
}

