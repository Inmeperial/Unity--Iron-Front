using UnityEngine;
using UnityEngine.Audio;

namespace GameSettings.Audio
{
    public class MuteToggle : SettingsToggle
    {
        [SerializeField] private AudioMixer _globalMixer;

        public override void OnSettingsLoaded()
        {
            base.OnSettingsLoaded();

            _toggle.isOn = Settings.Instance.SettingsData.mute;
        }
        protected override void OnSettingsChange()
        {
            Settings.Instance.SettingsData.mute = _toggle.isOn;
        }
        protected override void OnValueChange(bool status)
        {
            if (status)

                _globalMixer.SetFloat("MasterVolume", SoundUtilities.FloatToDB(0.0001f));
            else
            {
                float value = Settings.Instance.SettingsData.generalVolume;
                _globalMixer.SetFloat("MasterVolume", SoundUtilities.FloatToDB(value / 100));
            }

            OnSettingsChange();
        }
    }
}

