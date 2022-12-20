﻿namespace GameSettings.Audio
{
    public class FXVolume : VolumeSlider
    {
        protected override void Configure()
        {
            base.Configure();
            float volume = Settings.Instance.SettingsData.fxVolume;

            _slider.value = (int)volume;
        }

        protected override void ApplySetting()
        {
            base.ApplySetting();
            Settings.Instance.SettingsData.fxVolume = _slider.value;
        }
    }
}

