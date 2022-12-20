namespace GameSettings.Audio
{
    public class MusicVolume : VolumeSlider
    {
        protected override void Configure()
        {
            base.Configure();
            float volume = Settings.Instance.SettingsData.musicVolume;

            _slider.value = (int)volume;
        }

        protected override void ApplySetting()
        {
            base.ApplySetting();
            Settings.Instance.SettingsData.musicVolume = _slider.value;
        }
    }
}

