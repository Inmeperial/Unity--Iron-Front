using UnityEngine;

namespace GameSettings.Audio
{
    public class GeneralVolume : VolumeSlider
    {
        protected override void InitializeSliderValue()
        {
            float volume = Settings.Instance.SettingsData.generalVolume;

            _slider.value = (int)volume;
        }

        protected override void OnValueChange(float value)
        {
            _statusText.text = (int)value + "%";

            if (Settings.Instance.SettingsData.mute)
                value = _slider.minValue;

            _audioMixer.SetFloat(_volumeParameterName, SoundUtilities.FloatToDB(value / 100));
            OnSettingsChange();
        }

        protected override void OnSettingsChange()
        {
            Settings.Instance.SettingsData.generalVolume = _slider.value;
        }
    }
}

