﻿namespace GameSettings.Audio
{
    public class FXVolume : VolumeSlider
    {
        protected override void InitializeSliderValue()
        {
            float volume = Settings.Instance.SettingsData.fxVolume;

            _slider.value = (int)volume;
        }

        protected override void OnSettingsChange()
        {
            Settings.Instance.SettingsData.fxVolume = _slider.value;
        }
    }
}

