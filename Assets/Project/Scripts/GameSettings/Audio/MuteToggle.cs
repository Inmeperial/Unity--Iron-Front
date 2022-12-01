using UnityEngine;
using UnityEngine.Audio;

namespace GameSettings.Audio
{
    public class MuteToggle : SettingsToggle
    {
        [SerializeField] private AudioMixer _globalMixer;

        private float _volumeBeforeMute;

        public override void OnSettingsLoaded()
        {
            base.OnSettingsLoaded();

            _toggle.isOn = Settings.SettingsData.mute;
        }
        protected override void OnSettingsChange()
        {
            Settings.SettingsData.mute = _toggle.isOn;
        }
        protected override void OnValueChange(bool status)
        {
            if (status)
            {
                _globalMixer.GetFloat("Volume", out _volumeBeforeMute);
                _globalMixer.SetFloat("Volume", SoundUtilities.FloatToDB(0.0001f));
            }
            else
                _globalMixer.SetFloat("Volume", _volumeBeforeMute);

            OnSettingsChange();
        }

    }
}

